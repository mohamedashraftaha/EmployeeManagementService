using EmployeeManagementService_Backend.Domain.Models;
using EmployeeManagementService_Backend.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService_Backend.Service;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    public EmployeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<Employee>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        string? firstName = null,
        string? lastName = null,
        string? email = null,
        string? position = null
    )
    {
        try
        {
            var query = await _unitOfWork.employeeRepository.GetAllAsync();
            query = query.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(searchTerm) ||
                    e.LastName.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm) ||
                    e.Position.Contains(searchTerm));
            }
            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(e => e.FirstName.Contains(firstName));
            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(e => e.LastName.Contains(lastName));
            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(e => e.Email.Contains(email));
            if (!string.IsNullOrWhiteSpace(position))
                query = query.Where(e => e.Position.Contains(position));
            int totalCount = query.Count();
            var employees = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return new PagedResult<Employee>
            {
                Data = employees,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            // Optionally log ex
            throw;
        }
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        try
        {
            return await _unitOfWork.employeeRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            // Optionally log ex
            throw;
        }
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var created = await _unitOfWork.employeeRepository.AddAsync(employee);
            await _unitOfWork.CommitTransactionAsync();
            return created;
        }
        catch (Exception ex)
        {
            // Optionally log ex
            throw;
        }
    }

    public async Task<Employee?> UpdateAsync(int id, Employee employee)
    {
        try
        {
            if (id != employee.Id)
                return null;
            await _unitOfWork.BeginTransactionAsync();
            var updated = _unitOfWork.employeeRepository.Update(employee);
            if (updated == null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return null;
            }
            await _unitOfWork.CommitTransactionAsync();
            return updated;
        }
        catch (Exception ex)
        {
            // Optionally log ex
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var deleted = await _unitOfWork.employeeRepository.DeleteAsync(id);
            if (!deleted)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Optionally log ex
            throw;
        }
    }
} 