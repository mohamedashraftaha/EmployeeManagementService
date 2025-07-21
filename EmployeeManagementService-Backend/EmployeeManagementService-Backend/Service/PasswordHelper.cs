using System;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagementService_Backend.Service
{
    public static class PasswordHelper
    {
        public static string GenerateSalt(int size = 32)
        {
            var rng = new RNGCryptoServiceProvider();
            var saltBytes = new byte[size];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combined = Encoding.UTF8.GetBytes(password + salt);
                var hash = sha256.ComputeHash(combined);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string password, string salt, string hash)
        {
            var computedHash = HashPassword(password, salt);
            return computedHash == hash;
        }
    }
} 