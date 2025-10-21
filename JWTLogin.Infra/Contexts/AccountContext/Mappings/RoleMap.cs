using JWTLogin.Core.Contexts.AccountContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWTLogin.Infra.Contexts.AccountContext.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(120);
        }
    }
}
