using EmployeeManagementService_Backend.Domain.Models;

namespace EmployeeManagementService_Backend.Infrastructure.UsersRepository;


public class UsersRepository : IUsersRepository
{
    private readonly EmployeeDbContext _employeeDbContext;

    public UsersRepository(EmployeeDbContext employeeDbContext)
    {
        _employeeDbContext = employeeDbContext;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        try
        {
            return await _employeeDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<User> AddAsync(User user)
    {
        try
        {
            var entry = await _employeeDbContext.Users.AddAsync(user);
            return entry.Entity;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public User? Update(User user)
    {
        try
        {
            var entry = _employeeDbContext.Users.Update(user);
            return entry.Entity;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var user = await _employeeDbContext.Users.FindAsync(id);
            if (user == null)
                return false;
            _employeeDbContext.Users.Remove(user);
            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}