using FastEndpoints;
using FluentValidation;

namespace Authentication.Endpoints.SendEmailChangeToken
{
    public class SendEmailChangeTokenRequestValidator: Validator<SendEmailChangeTokenRequest>
    {
        public SendEmailChangeTokenRequestValidator()
        {
            RuleFor(x => x.EmailAddress).EmailAddress();
            RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
        }
    }
}
