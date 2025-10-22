using JWTLogin.Core.Contexts.AccountContext.Entities;

namespace JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts
{
    public interface IRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<Role>> GetUserRolesAsync(string email);
    }
}
