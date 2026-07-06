using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAssignment.Data;
using MyAssignment.Extensions;
using MyAssignment.Helper;
using MyAssignment.Services;

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

// Identity (login/credentials) and JWT bearer authentication
builder.Services.AddIdentityConfiguration();
builder.Services.AddJwtAuthentication(builder.Configuration);

// fallback authorization policy: every endpoint requires an authenticated
// user by default, unless explicitly marked [AllowAnonymous]
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Swagger, with JWT bearer support in the UI
builder.Services.AddSwaggerWithJwtSupport();

// AutoMapper registration for mapping between models and DTOs
builder.Services.AddAutoMapper(typeof(MappingProfile));

// register the IUserService interface and its implementation UserService for dependency injection
builder.Services.AddScoped<IUserService, UserService>();

// register the JWT token generation service for dependency injection
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// API Versioning configuration
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // tells client which versions are available in response headers
}).AddMvc();

// finalize registeration and build the runnable app
var app = builder.Build();

app.UseSwagger(); // enable swagger middleware
app.UseSwaggerUI(); // enable swagger UI middleware

// authentication must run before authorization in the middleware pipeline
app.UseAuthentication();
app.UseAuthorization();

// enable routing middleware to route incoming requests to the appropriate controller actions
app.MapControllers();

// start web server and listen for incoming requests
app.Run();