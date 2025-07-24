using JWTLogin.Core.SharedContext.Extensions;
using JWTLogin.Core.SharedContext.ValueObjects;
using System.Text.RegularExpressions;

namespace JWTLogin.Core.AccountContext.ValueObjects
{
    public partial class Email : ValueObject
    {
        private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public string Address { get; }
        public string Hash => Address.ToBase64();
        public Verification Verification { get; private set; } = new ();

        public void ResendVerification()
            => Verification = new Verification();

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new Exception("Email address cannot be null or empty");

            Address = address.Trim().ToLower();

            if(address.Length < 5)
                throw new Exception("Invalid email");

            if (!EmailRegex().IsMatch(Address))
                throw new ArgumentException("Invalid email format.", nameof(address));
        }

        public static implicit operator string(Email email) 
            => email.Address;

        public static implicit operator Email(string address) 
            => new (address);

        public override string ToString()
            => Address;

        [GeneratedRegex(Pattern)]
        private static partial Regex EmailRegex();
    }
}
