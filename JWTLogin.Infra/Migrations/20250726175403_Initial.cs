using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTLogin.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(120)", maxLength: 120, nullable: false),
                    EmailVerificationCode = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false),
                    EmailVerificationExpiresAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    EmailVerificationVerifiedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Name = table.Column<string>(type: "NVARCHAR(120)", maxLength: 120, nullable: false),
                    PasswordHash = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false, defaultValue: ""),
                    PasswordResetCode = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false),
                    Image = table.Column<string>(type: "NVARCHAR(150)", maxLength: 150, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
