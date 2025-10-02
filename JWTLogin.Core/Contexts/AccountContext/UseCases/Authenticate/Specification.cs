using Flunt.Notifications;
using Flunt.Validations;

namespace JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate
{
    public static class Specification
    {
        public static Contract<Notification> Ensure(Request request)
        {
            return new Contract<Notification>()
                .IsNotNull(request, "Request", "Request cannot be empty")
                .IsEmail(request.Email, "Email", "Invalid e-mail")
                .IsGreaterThan(request.Password?.Length ?? 0, 8, "Password", "Password must be at least 8 characters long")
                .IsLowerThan(request.Password?.Length ?? 0, 40, "Password", "Password must not exceed 40 characters");
        }
    }
}
