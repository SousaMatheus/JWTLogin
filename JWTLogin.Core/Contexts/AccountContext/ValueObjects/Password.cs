using JWTLogin.Core.Contexts.SharedContext.ValueObjects;
using System.Security.Cryptography;

namespace JWTLogin.Core.Contexts.AccountContext.ValueObjects
{
    public class Password : ValueObject
    {
        private const string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const string Special = "!@#$%ˆ&*(){}[];çÇ";

        public string Hash { get; } = string.Empty;
        public string ResetCode { get; } = Guid.NewGuid().ToString("N")[..8].ToUpper();

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
            var res = new char[length];

            for(var i = 0; i < length; i++)
            {
                var index = RandomNumberGenerator.GetInt32(chars.Length);
                res[i] = chars[index];
            }

            return upperCase
                ? new string(res).ToUpperInvariant()
                : new string(res);
        }

        private static string Hashing(
            string password,
            short saltSize = 16,
            short keySize = 64,
            int iterations = 10000,
            char splitChar = '.')
        {
            if(string.IsNullOrEmpty(password))
                throw new Exception("Password should not be null or empty");

            password += Configuration.Secrets.PasswordSaltKey;

            var salt = RandomNumberGenerator.GetBytes(saltSize);
            var derivedKey = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, keySize);

            return $"{iterations}{splitChar}{salt}{splitChar}{derivedKey}";
        }

        private static bool Verify(
        string hash,
        string password,
        short keySize = 64,
        int iterations = 10000,
        char splitChar = '.')
        {
            password += Configuration.Secrets.PasswordSaltKey;

            var parts = hash.Split(splitChar, 3);
            if(parts.Length != 3)
                return false;

            var hashIterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            if(hashIterations != iterations)
                return false;

            var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, keySize);

            return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
        }
    }
}
