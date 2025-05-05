using CurrencyApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;

namespace CurrencyApp.Data;

[ExcludeFromCodeCoverage]
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User Role Conversions
        // Define a value comparer for the Roles property to ensure EF Core can track changes.
        var roleComparer = new ValueComparer<List<UserRole>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => a ^ v.GetHashCode()),
            c => c.ToList());

        modelBuilder.Entity<User>()
            .Property(u => u.Roles)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(e => (UserRole)Enum.Parse(typeof(UserRole), e))
                      .ToList()
            )
            .Metadata.SetValueComparer(roleComparer);
        #endregion

        SeedData(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    internal void SeedData(ModelBuilder modelBuilder)
    {
        var users = new List<User>
            {
               new() {
                   Id = 1,
                   Name = "Admin",
                   Email="admin@app.com",
                   Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                   Roles = [UserRole.Admin, UserRole.Customer, UserRole.Analyst]
               },
                new() {
                   Id = 2,
                   Name = "Varun",
                   Email="varun@app.com",
                   Password = BCrypt.Net.BCrypt.HashPassword("Varun@123"),
                   Roles = [UserRole.Customer]
               },
                new() {
                   Id = 3,
                   Name = "Teja",
                   Email="teja@app.com",
                   Password = BCrypt.Net.BCrypt.HashPassword("Teja@123"),
                   Roles = [UserRole.Analyst, UserRole.Customer]
               },
            };

        modelBuilder.Entity<User>().HasData(users);
    }
}