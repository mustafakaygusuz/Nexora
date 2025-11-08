using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Nexora.Core.Common.Helpers
{
    public sealed class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 1000;

        public static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            var hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, Iterations, KeySize);

            var hashBytes = new byte[SaltSize + KeySize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string? hashedPassword, string password)
        {
            if (string.IsNullOrEmpty(hashedPassword))
            {
                return false;
            }

            var hashBytes = Convert.FromBase64String(hashedPassword);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var hash = new byte[KeySize];
            Array.Copy(hashBytes, SaltSize, hash, 0, KeySize);

            var testHash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, Iterations, KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, testHash);
        }
    }
}