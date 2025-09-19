using MediatR;

namespace JWTLogin.Core.Contexts.AccountContext.UseCases.Create
{
    public record Request(string Email, string Name, string Password) : IRequest<Response>;
}
