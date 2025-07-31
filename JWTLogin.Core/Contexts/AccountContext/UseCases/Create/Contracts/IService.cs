using JWTLogin.Core.Contexts.AccountContext.Entities;

namespace JWTLogin.Core.Contexts.AccountContext.UseCases.Create.Contracts
{
    public interface IService
    {
        Task SendValidationEmailAsync(User user, CancellationToken cancellationToken);
    }
}
