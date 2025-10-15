using JWTLogin.Core;
using JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTLogin.Api.Extensions
{
    public static class TokenExtensions
    {
        public static string GenerateToken(ResponseData data)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(Configuration.Secrets.JwtPrivateKey);

            var credential = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credential,
                Subject = GenerateClaims(data),
                Expires = DateTime.UtcNow.AddHours(9)
            };

            var token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(ResponseData user)
        {
            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim("Id", user.Id));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            foreach(var role in user.Roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return claimsIdentity;
        }
    }
}
