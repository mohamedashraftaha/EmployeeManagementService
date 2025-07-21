using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementService_Backend.Domain.Models;

public class Employee
{
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public required string FirstName { get; set; }
    [Required]
    [StringLength(50)]
    public required string LastName { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    [StringLength(100)]
    public required string Position { get; set; }
}
