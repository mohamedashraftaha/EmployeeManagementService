using EmployeeManagementService_Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService_Backend.Infrastructure.EmployeesRepository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _employeeDbContext;

    public EmployeeRepository(EmployeeDbContext employeeDbContext)
    {
        _employeeDbContext = employeeDbContext;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _employeeDbContext.Employees.ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _employeeDbContext.Employees.FindAsync(id);

    }

    public async Task<Employee> AddAsync (Employee employee)
    {
        var entry = await _employeeDbContext.Employees.AddAsync(employee);
        return entry.Entity;
    }

    public Employee? Update(Employee employee)
    {
        var entry =  _employeeDbContext.Employees.Update(employee);
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _employeeDbContext.Employees.FindAsync(id);
        if (employee == null)
            return false;

        _employeeDbContext.Employees.Remove(employee);
        return true;
    }
}
