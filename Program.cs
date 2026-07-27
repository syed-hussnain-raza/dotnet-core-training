using MyAssignment.Extensions;

// load .env into process environment variables before the builder reads
// configuration, so Smtp__Password becomes available as Smtp:Password
DotNetEnv.Env.Load();

// Create the builder
var builder = WebApplication.CreateBuilder(args);

// Register application architecture configurations and services
builder.Services.AddControllersWithFilters()
                .AddDatabaseContext(builder.Configuration)
                .AddApplicationOptions(builder.Configuration)
                .AddApplicationServices()
                .AddAppIdentity()
                .AddJwtAuthentication(builder.Configuration)
                .AddAuthorizationPolicies()
                .AddApiVersioningConfiguration()
                .AddSwaggerWithJwt();

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