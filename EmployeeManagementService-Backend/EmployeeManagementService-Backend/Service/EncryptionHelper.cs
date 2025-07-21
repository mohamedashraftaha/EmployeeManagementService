using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagementService_Backend.Service
{
    public static class EncryptionHelper
    {
        // For demonstration only. Store these securely in production!
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("ThisIsASecretKeyForAES123456789012"); // 32 bytes for AES-256
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("ThisIsAnIV123456"); // 16 bytes for AES

        public static string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                var buffer = Convert.FromBase64String(cipherText);
                using (var ms = new MemoryStream(buffer))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
} 