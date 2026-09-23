using JWTLogin.Core.Contexts.SharedContext.ValueObjects;
using System.Security.Cryptography;

namespace JWTLogin.Core.Contexts.AccountContext.ValueObjects
{
    public class Password : ValueObject
    {
        private const string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const string Special = "!@#$%ˆ&*(){}[];çÇ";

        public string Hash { get; } = string.Empty;

        public string ResetCode { get; } = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();

        protected Password()
        {
        }

        public Password(string password)
        {
            if(string.IsNullOrWhiteSpace(password))
                password = Generate();

            Hash = Hashing(password);
        }

        public bool Challenge(string plainTextPassword)
            => Verify(Hash, plainTextPassword);

        private static string Generate(
            short length = 16,
            bool includeSpecialChars = true,
            bool upperCase = false)
        {
            var chars = includeSpecialChars ? Valid + Special : Valid;
            var result = new char[length];

            for(var i = 0; i < length; i++)
            {
                var index = RandomNumberGenerator.GetInt32(chars.Length);
                result[i] = chars[index];
            }

            var password = new string(result);

            return upperCase ? password.ToUpperInvariant() : password;
        }

        private static string Hashing(
            string password,
            short saltSize = 16,
            short keySize = 32,
            int iterations = 10000,
            char splitChar = '.')
        {
            if(string.IsNullOrWhiteSpace(password))
                throw new Exception("Password should not be null or empty");

            password += Configuration.Secrets.PasswordSaltKey;

            var salt = RandomNumberGenerator.GetBytes(saltSize);

            var derivedKey = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                keySize);

            var saltBase64 = Convert.ToBase64String(salt);
            var keyBase64 = Convert.ToBase64String(derivedKey);

            return $"{iterations}{splitChar}{saltBase64}{splitChar}{keyBase64}";
        }

        private static bool Verify(
            string hash,
            string password,
            int iterations = 10000,
            char splitChar = '.')
        {
            if(string.IsNullOrWhiteSpace(hash) || string.IsNullOrWhiteSpace(password))
                return false;

            var parts = hash.Split(splitChar, 3);

            if(parts.Length != 3)
                return false;

            if(!int.TryParse(parts[0], out var hashIterations) || hashIterations != iterations)
                return false;

            byte[] salt;
            byte[] storedKey;

            try
            {
                salt = Convert.FromBase64String(parts[1]);
                storedKey = Convert.FromBase64String(parts[2]);
            }
            catch(FormatException)
            {
                return false;
            }

            if(salt.Length != 16 || storedKey.Length != 32)
                return false;

            password += Configuration.Secrets.PasswordSaltKey;

            var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                hashIterations,
                HashAlgorithmName.SHA256,
                storedKey.Length);

            return CryptographicOperations.FixedTimeEquals(keyToCheck, storedKey);
        }
    }
}