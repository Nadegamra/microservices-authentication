using FastEndpoints;
using FluentValidation;

namespace Authentication.Endpoints.Register
{
    public class RegisterRequestValidator : Validator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Custom((password, context) =>
            {
                if (password.Length < 8)
                {
                    context.AddFailure("Password must be at least 8 characters long");
                }
                if (!password.Any(char.IsUpper))
                {
                    context.AddFailure("Password must contain at least one uppercase letter");
                }
                if (!password.Any(char.IsLower))
                {
                    context.AddFailure("Password must contain at least one lowercase letter");
                }
                if (!password.Any(char.IsDigit))
                {
                    context.AddFailure("Password must contain at least one digit");
                }
            });
            RuleFor(x => x.Role).InclusiveBetween(0, 1);
        }
    }
}
