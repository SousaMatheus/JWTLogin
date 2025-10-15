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

            #region Authenticate
            builder.Services.AddTransient<
                JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts.IRepository,
                JWTLogin.Infra.Contexts.AccountContext.UseCases.Authenticate.Repository>();

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

            #region Authenticate

            app.MapPost("api/v1/authenticate", async (

                JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate.Request request,
                IRequestHandler<
                    JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate.Request,
                    JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate.Response> handler) =>
            {
                var result = await handler.Handle(request, new CancellationToken());

                if(!result.IsSuccess)                
                    return Results.Json(result, statusCode: result.Status);

                if(result.Data is null)
                    return Results.Json(result, statusCode: 500);
                
                result.Data.Token = TokenExtensions.GenerateToken(result.Data);
                return Results.Ok(result);
            });

            #endregion
        }
    }
}
