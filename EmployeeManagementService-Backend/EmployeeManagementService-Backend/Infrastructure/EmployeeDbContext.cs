using EmployeeManagementService_Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService_Backend.Infrastructure;

public class EmployeeDbContext : DbContext
{

    public DbSet<Employee> Employees { get; set; }

    public DbSet<User> Users { get; set; }

    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>().HasKey(e => e.Username);
        modelBuilder.Entity<Employee>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Employee>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        // Seed data
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com", Position = "Developer" },
            new Employee { Id = 2, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", Position = "Designer" },
            new Employee { Id = 3, FirstName = "Charlie", LastName = "Williams", Email = "charlie.williams@example.com", Position = "Manager" }
        );
    }
}
