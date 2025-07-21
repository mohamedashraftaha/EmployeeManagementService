using EmployeeManagementService_Backend.Domain.Models;
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
            new User { Username = "admin", Password = "password" },
            new User { Username = "user", Password = "password" }
        };

        public async Task<User?> ValidateUser(string username, string password)
        {
            var user = await _unitOfWork.usersRepository.GetByUsernameAsync(username);
            if (user == null)
                return null;
            var encryptedPassword = EncryptionHelper.Encrypt(password);
            if (user.PasswordHash == encryptedPassword)
                return user;
            return null;
        }

        public async Task<bool> RegisterUser(string username, string password, string? role = null)
        {
            var existingUser = await _unitOfWork.usersRepository.GetByUsernameAsync(username);
            if (existingUser != null)
                return false;
            var encryptedPassword = EncryptionHelper.Encrypt(password);
            var newUser = new User
            {
                Username = username,
                PasswordHash = encryptedPassword,
                Role = string.IsNullOrWhiteSpace(role) ? "User" : role
            };
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.usersRepository.AddAsync(newUser);
            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
    }
} 