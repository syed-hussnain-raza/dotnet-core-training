using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAssignment.Data;
using MyAssignment.Extensions;
using MyAssignment.Helper;
using MyAssignment.Repositories;
using MyAssignment.Services.Auth;
using MyAssignment.Services.Email;
using MyAssignment.Services.Users;
using MyAssignment.Options;

// load .env into process environment variables before the builder reads
// configuration, so Smtp__Password becomes available as Smtp:Password
DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

// Create the builder
var builder = WebApplication.CreateBuilder(args);

// register controllers so app look for controller class, with the global
// ModelState validation filter applied to every action
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateFilter>();
});

// disable the built-in automatic 400 response for invalid ModelState, so the
// global ValidateModelStateFilter handles it instead in our own ApiResponse<T> shape
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// register the SQL Server database context via dependency injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// register the open generic repository interface and its implementation for dependency injection
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register application services (Email, Identity, JWT, Swagger)
builder.Services.AddApplicationServices(builder.Configuration);

// Configuration Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection(nameof(UrlSettings)));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(nameof(SmtpSettings)));

// fallback authorization policy: every endpoint requires an authenticated
// user by default, unless explicitly marked [AllowAnonymous]
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// AutoMapper registration for mapping between models and DTOs
builder.Services.AddAutoMapper(typeof(MappingProfile));

// register the IUserService interface and its implementation UserService for dependency injection
builder.Services.AddScoped<IUserService, UserService>();

// register the IAuthService interface and its implementation AuthService for dependency injection
builder.Services.AddScoped<IAuthService, AuthService>();

// register the IEmailService interface and its implementation EmailService for dependency injection
builder.Services.AddScoped<IEmailService, EmailService>();

// register the JWT token generation service for dependency injection
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// API Versioning configuration
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
}).AddMvc();

// finalize registeration and build the runnable app
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// authentication must run before authorization in the middleware pipeline
app.UseAuthentication();
app.UseAuthorization();

// enable routing middleware to route incoming requests to the appropriate controller actions
app.MapControllers();

// start web server and listen for incoming requests
app.Run();