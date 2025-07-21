using EmployeeManagementService_Backend.Infrastructure.EmployeesRepository;

namespace EmployeeManagementService_Backend.Infrastructure.UnitOfWork;

public interface IUnitOfWork
{

    Task<int> CommitTransactionAsync();
    Task BeginTransactionAsync();
    Task RollbackTransactionAsync();

    IEmployeeRepository employeeRepository { get; }
    public void Dispose();
}
