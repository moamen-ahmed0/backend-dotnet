
var builder = WebApplication.CreateBuilder(args); // Create the application builder
builder.Services.AddOpenApi();    // Add OpenAPI
builder.Services.AddSwaggerGen(); // Add Swagger
var app = builder.Build();        // Build the application

app.MapOpenApi();  // Enable OpenAPI endpoint, json document that describes your API
app.UseSwagger();  // Enable Swagger UI, UI uses this json document.
app.UseSwaggerUI();

// In-memory users list
var users = new List<User>
{
    new User(1, "moamen", "123456"),
    new User(2, "ahmed", "complexpassword"),
};

// GET /users - Return all users
app.MapGet("/users", () => users);

// Start the application
app.Run();

// User model
record User(int Id, string Username, string Password);
