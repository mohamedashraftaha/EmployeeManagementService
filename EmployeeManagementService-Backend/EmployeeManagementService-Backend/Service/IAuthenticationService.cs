using EmployeeManagementService_Backend.Domain.Models;

namespace EmployeeManagementService_Backend.Service
{
    public interface IAuthenticationService
    {
        Task<User?> ValidateUser(string username, string password);
        Task<bool> RegisterUser(string username, string password, string? role = null);
    }
} 