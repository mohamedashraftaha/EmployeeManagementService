using EmployeeManagementService_Backend.Domain.Models;

namespace EmployeeManagementService_Backend.Service
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
} 