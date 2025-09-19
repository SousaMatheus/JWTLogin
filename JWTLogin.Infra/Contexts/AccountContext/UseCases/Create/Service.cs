using JWTLogin.Core;
using JWTLogin.Core.Contexts.AccountContext.Entities;
using JWTLogin.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace JWTLogin.Infra.Contexts.AccountContext.UseCases.Create
{
    public class Service : IService
    {
        public async Task SendValidationEmailAsync(User user, CancellationToken cancellationToken)
        {
            var client = new SendGridClient(Configuration.SendGrid.ApiKey);
            var from = new EmailAddress(Configuration.Email.DefaultFromEmail, Configuration.Email.DefaultFromName);
            var subject = "Please verify your email address";
            var to = new EmailAddress(user.Email, user.Name);
            var plainTextContent = $"Code {user.Email.Verification.Code}";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, plainTextContent);

            await client.SendEmailAsync(msg, cancellationToken);
        }
    }
}
