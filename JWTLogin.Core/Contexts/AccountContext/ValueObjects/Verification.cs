using JWTLogin.Core.Contexts.SharedContext.ValueObjects;

namespace JWTLogin.Core.Contexts.AccountContext.ValueObjects
{
    public class Verification : ValueObject
    {
        public string Code { get; } = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
        public DateTime? ExpiresAt { get; private set; } = DateTime.UtcNow.AddMinutes(5);
        public DateTime? VerifiedAt { get; private set; } = null;
        public bool IsActive => VerifiedAt != null && ExpiresAt == null;

        public Verification()
        {
        }

        public void Verify(string code)
        {
            if(IsActive)
                throw new InvalidOperationException("Verification is already active.");

            if(ExpiresAt < DateTime.UtcNow)
                throw new InvalidOperationException("Verification code has expired.");

            if(!string.Equals(code.Trim(), Code.Trim(), StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ArgumentException("Invalid verification code.", nameof(code));
            }

            VerifiedAt = DateTime.UtcNow;
            ExpiresAt = null;
        }
    }
}
