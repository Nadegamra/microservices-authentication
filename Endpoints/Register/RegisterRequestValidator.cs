using FastEndpoints;
using FluentValidation;

namespace Authentication.Endpoints.Register
{
    public class RegisterRequestValidator: Validator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).MinimumLength(8);
        }
    }
}
