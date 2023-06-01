using FastEndpoints;
using FluentValidation;

namespace Authentication.Endpoints.SendPasswordResetToken
{
    public class SendPasswordResetTokenRequestValidator: Validator<SendPasswordResetTokenRequest>
    {
        public SendPasswordResetTokenRequestValidator()
        {
            RuleFor(x => x.EmailAddress).EmailAddress();
        }
    }
}
