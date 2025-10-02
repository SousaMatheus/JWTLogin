using MediatR;

namespace JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate
{
    public record Request(string Email, string Password) : IRequest<Response>;
}
