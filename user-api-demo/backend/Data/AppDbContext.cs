using Microsoft.EntityFrameworkCore;
using user_api_demo.Models;

namespace user_api_demo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>(); // Expose the Users table to EF Core
}

// AppDbContext: connects to MySQL, exposes tables (DbSet<T>) for EF Core.
// Bridge between your .NET application and your MySQL database.
