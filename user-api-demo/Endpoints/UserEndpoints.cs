using Microsoft.EntityFrameworkCore;
using user_api_demo.Data;
using user_api_demo.Models;

namespace user_api_demo.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        // GET /users
        app.MapGet("/users", async (AppDbContext db) =>
        {
            return await db.Users.ToListAsync();
        });

        // POST /users
        app.MapPost("/users", async (CreateUserRequest request, AppDbContext db) =>
        {
            var user = new User
            {
                Username = request.Username,
                Password = request.Password
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Results.Created($"/users/{user.Id}", user);
        });

        // PUT /users/{id}
        app.MapPut("/users/{id}", async (int id, UpdateUserRequest request, AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);

            if (user is null)
                return Results.NotFound("User not found.");

            user.Username = request.Username;
            user.Password = request.Password;

            await db.SaveChangesAsync();

            return Results.Ok(user);
        });

        // DELETE /users/{id}
        app.MapDelete("/users/{id}", async (int id, AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);

            if (user is null)
                return Results.NotFound("User not found.");

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
