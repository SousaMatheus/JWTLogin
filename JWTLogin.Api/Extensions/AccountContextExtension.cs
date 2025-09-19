using MediatR;

namespace JWTLogin.Api.Extensions
{
    public static class AccountContextExtension
    {
        public static void AddAccountContext(this WebApplicationBuilder builder)
        {
            #region Create

            builder.Services.AddTransient<
                JWTLogin.Core.Contexts.AccountContext.UseCases.Create.Contracts.IRepository,
                JWTLogin.Infra.Contexts.AccountContext.UseCases.Create.Repository>();

            builder.Services.AddTransient<
                JWTLogin.Core.Contexts.AccountContext.UseCases.Create.Contracts.IService,
                JWTLogin.Infra.Contexts.AccountContext.UseCases.Create.Service>();

            #endregion
        }

        public static void MapAccountEndpoints(this WebApplication app)
        {
            #region Create

            app.MapPost("api/v1/users", async (

                JWTLogin.Core.Contexts.AccountContext.UseCases.Create.Request request,
                IRequestHandler< //handler
                    JWTLogin.Core.Contexts.AccountContext.UseCases.Create.Request, //tipo do request
                    JWTLogin.Core.Contexts.AccountContext.UseCases.Create.Response> handler) => //handler
            {
                var result = await handler.Handle(request, new CancellationToken()); //processa a requisicao

                return result.IsSuccess
                ? Results.Created($"api/v1/users/{result.Data?.Id}", result)
                : Results.Json(result, statusCode: result.Status);
            });

            #endregion
        }
    }
}
