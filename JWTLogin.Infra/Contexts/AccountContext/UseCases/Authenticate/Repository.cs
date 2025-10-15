using JWTLogin.Core.Contexts.AccountContext.Entities;
using JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using JWTLogin.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace JWTLogin.Infra.Contexts.AccountContext.UseCases.Authenticate
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
            => await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Address.Equals(email, StringComparison.InvariantCultureIgnoreCase));
    }
}
