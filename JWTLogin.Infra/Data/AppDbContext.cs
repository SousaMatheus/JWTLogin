using JWTLogin.Core.Contexts.AccountContext.Entities;
using JWTLogin.Infra.Contexts.AccountContext.Mappings;
using Microsoft.EntityFrameworkCore;

namespace JWTLogin.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
