using JWTLogin.Core.SharedContext.Extensions;
using JWTLogin.Core.SharedContext.ValueObjects;

namespace JWTLogin.Core.AccountContext.ValueObjects
{
    public class Email : ValueObject
    {
        private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public string Address { get; }
        public string Hash => Address.ToBase64();
    }
}
