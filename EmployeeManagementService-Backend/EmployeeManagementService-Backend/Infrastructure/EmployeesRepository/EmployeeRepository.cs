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
        try
        {
            return await _employeeDbContext.Employees.ToListAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        try
        {
            return await _employeeDbContext.Employees.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Employee> AddAsync (Employee employee)
    {
        try
        {
            var entry = await _employeeDbContext.Employees.AddAsync(employee);
            return entry.Entity;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public Employee? Update(Employee employee)
    {
        try
        {
            var entry =  _employeeDbContext.Employees.Update(employee);
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
            var employee = await _employeeDbContext.Employees.FindAsync(id);
            if (employee == null)
                return false;

            _employeeDbContext.Employees.Remove(employee);
            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
