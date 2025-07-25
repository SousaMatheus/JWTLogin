using System.Text;

namespace JWTLogin.Core.Contexts.SharedContext.Extensions
{
    public static class StringExtension
    {
        public static string ToBase64(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }
    }
}
