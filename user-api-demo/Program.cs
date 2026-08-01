// Create the web application
var builder = WebApplication.CreateBuilder(args);

// Add OpenAPI support (API documentation)
builder.Services.AddOpenApi();

// Add Swagger services
builder.Services.AddSwaggerGen();

// Build the application
var app = builder.Build();

// Generate the OpenAPI document
app.MapOpenApi();

// Enable the Swagger UI to test the API
app.UseSwagger();
app.UseSwaggerUI();

// Store users in memory (data is lost when the app stops)
var users = new List<User>
{
    new User(1, "moamen", "123456"),
    new User(2, "ahmed", "complexpassword"),
};

// GET /users - Return all users
app.MapGet("/users", () => users);

// POST /users - Create a new user
app.MapPost("/users", (CreateUserRequest request) =>
{
    var newUser = new User(
        users.Count + 1,
        request.Username,
        request.Password
    );

    users.Add(newUser);

    // Return HTTP 201 Created with the new user
    return Results.Created($"/users/{newUser.Id}", newUser);
});

// PUT /users/{id} - Update a user by ID
app.MapPut("/users/{id}", (int id, UpdateUserRequest request) =>
{
    // 1. Find the user with the given ID
    var user = users.FirstOrDefault(u => u.Id == id);

    // 2. If the user doesn't exist, return 404 Not Found
    if (user is null)
    {
        return Results.NotFound("User not found.");
    }

    // 3. Create a new user object with the updated data
    var updatedUser = user with
    {
        Username = request.Username,
        Password = request.Password
    };

    // 4. Replace the old user in the list with the updated one
    var index = users.IndexOf(user);
    users[index] = updatedUser;

    return Results.Ok(updatedUser);
});

// DELETE /users/{id} - Delete a user by ID
app.MapDelete("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);

    // Return 404 if the user does not exist
    if (user is null)
    {
        return Results.NotFound("User not found.");
    }

    // Remove the user from the list
    users.Remove(user);

    // Return HTTP 204 No Content
    return Results.NoContent();
});

// Start the application
app.Run();

// User model
record User(int Id, string Username, string Password);

// Request body for creating a user
record CreateUserRequest(string Username, string Password);

// Request body for updating a user
record UpdateUserRequest(string Username, string Password);