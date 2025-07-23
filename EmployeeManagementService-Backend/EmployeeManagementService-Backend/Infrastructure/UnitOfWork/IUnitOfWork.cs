using EmployeeManagementService_Backend.Infrastructure.EmployeesRepository;
using EmployeeManagementService_Backend.Infrastructure.UsersRepository;

namespace EmployeeManagementService_Backend.Infrastructure.UnitOfWork;

public interface IUnitOfWork
{

    Task<int> CommitTransactionAsync();
    Task BeginTransactionAsync();
    Task RollbackTransactionAsync();

    IEmployeeRepository employeeRepository { get; }
    IUsersRepository usersRepository { get; }
    public void Dispose();
}
