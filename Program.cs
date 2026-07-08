// Create the builder
var builder = WebApplication.CreateBuilder(args);

// register controllers so app look for controller class
builder.Services.AddControllers();

// finalize registeration and build the runnable app
var app = builder.Build();

// map controller endpoints
app.MapControllers();

// start web server and listen for incoming requests
app.Run();