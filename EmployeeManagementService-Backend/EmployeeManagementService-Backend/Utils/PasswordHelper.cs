using System;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagementService_Backend.Utils
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public static bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            var inputPasswordHash = HashPassword(inputPassword);
            return inputPasswordHash == storedPasswordHash;
        }
    }
}