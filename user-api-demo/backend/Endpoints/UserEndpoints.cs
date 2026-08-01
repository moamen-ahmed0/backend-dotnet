using Microsoft.EntityFrameworkCore;
using user_api_demo.Data;
using user_api_demo.Models;

namespace user_api_demo.Endpoints;

public static class UserEndpoints
{
    // Extension method: lets Program.cs just call app.MapUserEndpoints()
    // instead of writing all the routes there directly.
    public static void MapUserEndpoints(this WebApplication app)
    {
        // GET /users -> return every user (SELECT * FROM Users)
        app.MapGet("/users", async (AppDbContext db) =>
        {
            return await db.Users.ToListAsync();
        });

        // POST /users -> create a new user from the request body
        app.MapPost("/users", async (CreateUserRequest request, AppDbContext db) =>
        {
            // Build the entity in memory first (not saved yet)
            var user = new User
            {
                Username = request.Username,
                Password = request.Password
            };

            // Stage the insert, then send it to MySQL
            db.Users.Add(user);
            await db.SaveChangesAsync();

            // 201 Created + the new user (now has an Id from the DB)
            return Results.Created($"/users/{user.Id}", user);
        });

        // PUT /users/{id} -> update an existing user
        app.MapPut("/users/{id}", async (int id, UpdateUserRequest request, AppDbContext db) =>
        {
            // Look the user up first (SELECT * FROM Users WHERE Id = id)
            var user = await db.Users.FindAsync(id);

            // No user with that id? 404
            if (user is null)
                return Results.NotFound("User not found.");

            // Change the in-memory object...
            user.Username = request.Username;
            user.Password = request.Password;

            // ...then send the UPDATE to MySQL
            await db.SaveChangesAsync();

            return Results.Ok(user);
        });

        // DELETE /users/{id} -> remove a user
        app.MapDelete("/users/{id}", async (int id, AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);

            if (user is null)
                return Results.NotFound("User not found.");

            // Stage the delete, then send it to MySQL
            db.Users.Remove(user);
            await db.SaveChangesAsync();

            // 204 = success, nothing to return
            return Results.NoContent();
        });
    }
}

// UserEndpoints: defines the /users API routes (GET, POST, PUT, DELETE).
