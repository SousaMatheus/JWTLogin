using JWTLogin.Core.Contexts.AccountContext.Entities;
using JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using MediatR;

namespace JWTLogin.Core.Contexts.AccountContext.UseCases.Authenticate
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IRepository _repository;

        public Handler(IRepository repository)
        {
            _repository = repository;
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
            catch(Exception)
            {
                return new Response("An error occured when trying to validate your request", 500);
            }
            #endregion

            User? user;

            #region 02 - Check if the user exists
            try
            {
                user = await _repository.GetUserByEmailAsync(request.Email);

                if(user is null)
                    return new Response("Invalid e-mail or password", 400);

            }
            catch(Exception)
            {
                return new Response("An error occured when trying to find your User", 500);
            }

            #endregion

            #region 03 - Check if the password is correct

            try
            {
                if(user.Password.Challenge(request.Password))
                    return new Response("Invalid e-mail or password", 400);
            }
            catch(Exception)
            {
                return new Response("An error occured when trying to check your password", 500);
            }

            #endregion

            #region 04 - Check if the user is verified
            try
            {
                if(!user.Email.Verification.IsActive)
                    return new Response("Inactive account", 400);
            }
            catch(Exception)
            {
                return new Response("An error occured when trying to check your profile", 500);
            }
            #endregion

            #region 05 - Get the user's roles
            try
            {
                user.Roles = await _repository.GetUserRolesAsync(request.Email);
            }
            catch(Exception)
            {
                return new Response("An error occured when trying to check your profile", 500);
            }
            #endregion

            #region 05 - Return the data
            var data = new ResponseData
            {
                Email = request.Email,
                Id = user.Id.ToString(),
                Name = user.Name,
                Roles = user.Roles.Select(x => x.Name).ToArray()
            };

            return new Response("", data);


            #endregion
        }
    }
}