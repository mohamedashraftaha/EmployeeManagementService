using EmployeeManagementService_Backend.Domain.Models;

namespace EmployeeManagementService_Backend.Infrastructure.EmployeesRepository;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> AddAsync(Employee employee);
    Employee? Update(Employee employee);
    Task<bool> DeleteAsync(int id);
}

