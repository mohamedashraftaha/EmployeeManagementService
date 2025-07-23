using EmployeeManagementService_Backend.Domain.Models;
using EmployeeManagementService_Backend.Infrastructure.UnitOfWork;
using EmployeeManagementService_Backend.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementService_Backend.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(ILogger<AuthenticationService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        private static readonly List<User> Users = new()
        {
            new User { Username = "admin", PasswordHash = "password" },
            new User { Username = "user", PasswordHash = "password" }
        };

        public async Task<User?> ValidateUser(string username, string password)
        {
            var user = await _unitOfWork.usersRepository.GetByUsernameAsync(username);
            if (user == null)
                return null;
            var hashedPassword = PasswordHelper.HashPassword(password);
            if (user.PasswordHash == hashedPassword)
                return user;
            return null;
        }

        public async Task<bool> RegisterUser(string username, string password, string? role = null)
        {
            var existingUser = await _unitOfWork.usersRepository.GetByUsernameAsync(username);
            if (existingUser != null)
                return false;
            var hashedPassword = PasswordHelper.HashPassword(password);
            var newUser = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Role = string.IsNullOrWhiteSpace(role) ? "User" : role
            };
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.usersRepository.AddAsync(newUser);
            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
    }
} 