using JWTLogin.Core.AccountContext.ValueObjects;
using JWTLogin.Core.Contexts.AccountContext.ValueObjects;
using JWTLogin.Core.Contexts.SharedContext.Entities;

namespace JWTLogin.Core.Contexts.AccountContext.Entities
{
    public class User : Entity
    {
        public Email Email { get; private set; } = null!;
        public string Name { get; private set; } = string.Empty;
        public Password Password { get; private set; } = null!;
        public string Image { get; set; } = string.Empty;
        public List<Role> Roles = new();

        protected User()
        {
        }

        public User(string email, string password)
        {
            Email = new Email(email);
            Password = new Password(password);
        }

        public User(string name, Email email, Password password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public void UpdatePassword(string plainTextPassword, string code)
        {
            if(string.Equals(code.Trim(), Password.ResetCode.Trim(), StringComparison.CurrentCultureIgnoreCase))
                throw new Exception("Invalid reset code!");

            Password = new Password(plainTextPassword);
        }

        public void UpdateEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                throw new Exception("Email cannot be empty.");

            Email = new Email(email);
        }

        public void ChangePassword(string plainTextPassword)
        {
            if(string.IsNullOrWhiteSpace(plainTextPassword))
                throw new Exception("Password cannot be empty.");

            Password = new Password(plainTextPassword);
        }
    }
}
