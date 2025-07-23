using EmployeeManagementService_Backend.Infrastructure.EmployeesRepository;
using EmployeeManagementService_Backend.Infrastructure.UsersRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeManagementService_Backend.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EmployeeDbContext _employeeDbContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public IEmployeeRepository employeeRepository => new EmployeeRepository(_employeeDbContext);
        public IUsersRepository usersRepository => new UsersRepository.UsersRepository(_employeeDbContext);

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                return;

            _transaction = await _employeeDbContext.Database.BeginTransactionAsync();
        }
        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
                return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task<int> CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                return 0;
            }

            try
            {
                int result = await _employeeDbContext.SaveChangesAsync();
                await _transaction.CommitAsync();
                return result;
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}
