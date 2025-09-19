using JWTLogin.Core.AccountContext.ValueObjects;
using JWTLogin.Core.Contexts.AccountContext.Entities;
using JWTLogin.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using JWTLogin.Core.Contexts.AccountContext.ValueObjects;
using MediatR;

namespace JWTLogin.Core.Contexts.AccountContext.UseCases.Create
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IRepository _repository;
        private readonly IService _service;

        public Handler(IRepository repository, IService service)
        {
            _repository = repository;
            _service = service;
        }
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            #region 01 - Validate request
            try
            {
                var response = Specification.Ensure(request);

                if(!response.IsValid)
                    return new Response("Invalid request", 400, response.Notifications);
            }
            catch
            {
                return new Response("An error occured when trying to validate your request", 500);
            }
            #endregion

            #region 02 - Generate objects
            Email email;
            Password password;
            User user;

            try
            {
                email = new Email(request.Email);
                password = new Password(request.Password);
                user = new User(request.Name, email, password);
            }
            catch(Exception ex)
            {
                return new Response(ex.Message, 500);
            }
            #endregion

            #region 03 - Check if the user already exists
            try
            {
                var exists = await _repository.AnyAsync(request.Email, cancellationToken);

                if(exists)
                    return new Response("E-mail already in use", 400);
            }
            catch
            {
                return new Response("An error occurred when while checking if the user already exists", 500);
            }
            #endregion

            #region 04 - Create the user
            try
            {
                await _repository.CreateAsync(user, cancellationToken);
            }
            catch
            {
                return new Response("An error occured while trying to create the user", 500);
            }
            #endregion

            #region 05 - Send the validation email
            try
            {
                await _service.SendValidationEmailAsync(user, cancellationToken);
            }
            catch
            {
                //do nothing
            }
            #endregion

            return new Response("User created successfully", new ResponseData(user.Id, user.Name, user.Email));
        }
    }
}
