using Flunt.Notifications;
using Flunt.Validations;

namespace JWTLogin.Core.Contexts.AccountContext.UseCases.Create
{
    public static class Specification
    {
        public static Contract<Notification> Ensure (Request request)
        {
            return new Contract<Notification>()
                .Requires()
                .IsNotNull(request, "Request", "Request cannot be null")
                .IsLowerThan(request.Name.Length, 120, "Name", "Name must have less than 120 characters")
                .IsGreaterThan(request.Name.Length, 3, "Name", "Name must have more than 3 characters")
                .IsNotNullOrEmpty(request.Name, "Name", "Name cannot be empty")
                .IsEmail(request.Email, "Email", "Email is not valid")
                .IsNotNullOrEmpty(request.Password, "Password", "Password cannot be empty")
                .IsGreaterThan(request.Password, 8, "Password", "Password must have at least 8 characters")
                .IsLowerThan(request.Password, 40, "Password", "Password mus have less than 40 characters");
        }
    }
}
