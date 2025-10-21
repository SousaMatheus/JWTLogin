using JWTLogin.Core.Contexts.AccountContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWTLogin.Infra.Contexts.AccountContext.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(120);

            builder.Property(u => u.Image)
                .HasColumnName("Image")
                .IsRequired()
                .HasColumnType("NVARCHAR")
                .HasMaxLength(150)
                .HasDefaultValue(string.Empty);

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address)
                  .HasColumnName("Email")
                  .IsRequired()
                  .HasColumnType("VARCHAR")
                  .HasMaxLength(120);

                email.HasIndex(email => email.Address)
                    .IsUnique()
                    .HasDatabaseName("IX_User_Email");

                email.OwnsOne(e => e.Verification)
                    .Property(v => v.Code)
                    .HasColumnName("EmailVerificationCode")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(10)
                    .IsRequired();

                email.OwnsOne(e => e.Verification)
                    .Property(v => v.ExpiresAt)
                    .HasColumnName("EmailVerificationExpiresAt")
                    .HasColumnType("DATETIME")
                    .IsRequired(false);

                email.OwnsOne(e => e.Verification)
                    .Property(v => v.VerifiedAt)
                    .HasColumnName("EmailVerificationVerifiedAt")
                    .HasColumnType("DATETIME")
                    .IsRequired(false);

                email.OwnsOne(e => e.Verification)
                    .Ignore(v => v.IsActive);
            });

            builder.OwnsOne(u => u.Password, password =>
            {
                password.Property(p => p.Hash)
                    .HasColumnName("PasswordHash")
                    .HasMaxLength(255)
                    .HasColumnType("VARCHAR")
                    .HasDefaultValue(string.Empty)
                    .IsRequired();
                password.Property(p => p.ResetCode)
                    .HasColumnName("PasswordResetCode")
                    .HasMaxLength(10)
                    .HasColumnType("VARCHAR")
                    .IsRequired();
            });

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                role => role
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade),
                user => user
                .HasOne<User>()
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade));
        }
    }
}