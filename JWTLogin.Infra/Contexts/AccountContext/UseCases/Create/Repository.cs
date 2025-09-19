using JWTLogin.Core.Contexts.AccountContext.Entities;
using JWTLogin.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using JWTLogin.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace JWTLogin.Infra.Contexts.AccountContext.UseCases.Create
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AnyAsync(string email, CancellationToken cancellationToken) 
            => await _context.Users
                            .AnyAsync(u => u.Email.Address == email, cancellationToken);

        public async Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
