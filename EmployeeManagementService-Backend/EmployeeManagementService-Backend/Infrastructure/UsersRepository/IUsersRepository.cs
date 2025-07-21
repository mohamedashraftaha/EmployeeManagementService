using EmployeeManagementService_Backend.Domain.Models;

namespace EmployeeManagementService_Backend.Infrastructure.UsersRepository;


public interface IUsersRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
    User? Update(User user);
    Task<bool> DeleteAsync(int id);
}