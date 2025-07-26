namespace JWTLogin.Core
{
    public static class Configuration
    {
        public static SecretsConfiguration Secrets { get; set; } = new SecretsConfiguration();
        public static DatabaseConfiguration Database { get; set; } = new DatabaseConfiguration();

        public class SecretsConfiguration
        {
            public string ApiKey { get; set; } = string.Empty;
            public string JwtPrivateKey { get; set; } = string.Empty;
            public string PasswordSaltKey { get; set; } = string.Empty;
        }

        public class DatabaseConfiguration
        {
            public string ConnectionString { get; set; } = string.Empty;
        }
    }
}
