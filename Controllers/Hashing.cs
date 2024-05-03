using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace ThreeFriends.Controllers
{
    public static class Hashing
    {
        private const int SaltSize = 16; // 128 bits
        private const int Iterations = 10000;
        private const int KeySize = 32; // 256 bits

        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, Iterations, KeySize);
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        public static bool ValidatePassword(string password, string hashedPassword)
        {
            string[] parts = hashedPassword.Split(':');
            if (parts.Length != 2)
            {
                return false; // Invalid hashed password format
            }

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hash = Convert.FromBase64String(parts[1]);

            byte[] newHash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, Iterations, KeySize);

            return hash.SequenceEqual(newHash);
        }
    }
}
