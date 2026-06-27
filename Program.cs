using MyAssignment.Helper;

// Create the builder
var builder = WebApplication.CreateBuilder(args);

// register controllers so app look for controller class
builder.Services.AddControllers();

// register services for swagger to generate API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper registration for mapping between models and DTOs
builder.Services.AddAutoMapper(typeof(MappingProfile));


//  finalize registeration and build the runnable app
var app = builder.Build();

app.UseSwagger(); // enable swagger middleware
app.UseSwaggerUI(); // enable swagger UI middleware

// 
app.MapControllers();

//  start web server and listen for incoming requests
app.Run();