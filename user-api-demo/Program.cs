
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

// POST /users - Add a new user
app.MapPost("/users", (CreateUserRequest request) =>
{
    var newUser = new User(
        users.Count + 1,
        request.Username,
        request.Password
    );

    users.Add(newUser);

    return Results.Created($"/users/{newUser.Id}", newUser);
});

// PUT /users/{id} - Update an existing user
app.MapPut("/users/{id}", (int id, UpdateUserRequest request) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);

    if (user is null)
    {
        return Results.NotFound("User not found.");
    }

    var updatedUser = user with
    {
        Username = request.Username,
        Password = request.Password
    };

    var index = users.IndexOf(user);
    users[index] = updatedUser;

    return Results.Ok(updatedUser);
});

// DELETE /users/{id} - Delete a user by ID
app.MapDelete("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);

    if (user is null)
    {
        return Results.NotFound("User not found.");
    }

    users.Remove(user);

    return Results.NoContent();
});

// Start the application
app.Run();

// User model
record User(int Id, string Username, string Password);

// Request model for creating a user
record CreateUserRequest(string Username, string Password);

// Request model for updating a user
record UpdateUserRequest(string Username, string Password);
