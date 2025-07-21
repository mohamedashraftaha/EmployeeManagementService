using EmployeeManagementService_Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService_Backend.Infrastructure;

public class EmployeeDbContext : DbContext
{

    public DbSet<Employee> Employees { get; set; }

    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Employee>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
    }
}
