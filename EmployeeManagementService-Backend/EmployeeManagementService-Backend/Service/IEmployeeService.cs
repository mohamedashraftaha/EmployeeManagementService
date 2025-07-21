using EmployeeManagementService_Backend.Domain.Models;

namespace EmployeeManagementService_Backend.Service;

public interface IEmployeeService
{
    Task<PagedResult<Employee>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        string? position = null
    );
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> AddAsync(Employee employee);
    Task<Employee?> UpdateAsync(int id, Employee employee);
    Task<bool> DeleteAsync(int id);
} 