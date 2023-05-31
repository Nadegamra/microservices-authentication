using FastEndpoints;
using FluentValidation.Results;

namespace Authentication.Endpoints.Login
{
    public class LoginRequestValidator : Validator<LoginRequest>
    {
        public override ValidationResult Validate(FluentValidation.ValidationContext<LoginRequest> context)
        {
            return base.Validate(context);
        }
    }
}
