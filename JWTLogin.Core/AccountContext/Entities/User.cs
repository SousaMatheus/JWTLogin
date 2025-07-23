using JWTLogin.Core.SharedContext.Entities;

namespace JWTLogin.Core.AccountContext.Entities
{
    public class User : Entity
    {
        public string Email { get; set; }
    }
}
