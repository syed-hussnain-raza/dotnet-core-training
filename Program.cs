using Microsoft.AspNetCore.Mvc;
using MyAssignment.Helper;
using MyAssignment.Services;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using MyAssignment.Data;

// Create the builder
var builder = WebApplication.CreateBuilder(args);

// register controllers so app look for controller class
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelStateFilter>();
});

// register the AppDbContext with SQL Server provider and connection string from configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// disable the built-in automatic 400 response for invalid ModelState, so the
// controller can check ModelState.IsValid itself and return validation
// errors wrapped in our own ApiResponse<T> shape instead of the default
// ValidationProblemDetails format
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// register services for swagger to generate API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper registration for mapping between models and DTOs
builder.Services.AddAutoMapper(typeof(MappingProfile));

// register the IUserService interface and its implementation UserService for dependency injection
builder.Services.AddScoped<IUserService, UserService>();

// API Versioning configuration
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // tells client which versions are available in response headers
}).AddMvc();


// finalize registeration and build the runnable app
var app = builder.Build();

app.UseSwagger(); // enable swagger middleware
app.UseSwaggerUI(); // enable swagger UI middleware

// enable routing middleware to route incoming requests to the appropriate controller actions
app.MapControllers();

// start web server and listen for incoming requests
app.Run();