// Program.cs
// This is the main entry point for the User API Demo application.
using Microsoft.EntityFrameworkCore;
using user_api_demo.Data;
using user_api_demo.Endpoints;

// Create the web application
var builder = WebApplication.CreateBuilder(args);

// Add OpenAPI support
builder.Services.AddOpenApi();

// Add Swagger
builder.Services.AddSwaggerGen();

// Configure MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});

// Build the application
var app = builder.Build();

// Create the database/tables if they don't exist yet
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Enable OpenAPI & Swagger
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

// Register the /users endpoints
app.MapUserEndpoints();

app.Run();

// Program.cs: starts the app, wires up services (Swagger, MySQL), ensures the DB schema exists, registers endpoints.
