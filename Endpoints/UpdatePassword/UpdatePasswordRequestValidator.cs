using FastEndpoints;
using FluentValidation;

namespace Authentication.Endpoints.UpdatePassword
{
    public class UpdatePasswordRequestValidator : Validator<UpdatePasswordRequest>
    {
        public UpdatePasswordRequestValidator()
        {
            RuleFor(x => x.OldPassword).Custom((password, context) =>
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
                if (!password.Any(char.IsSymbol))
                {
                    context.AddFailure("Password must contain at least one symbol");
                }
            });
            RuleFor(x => x.NewPassword).Custom((password, context) =>
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
                if (!password.Any(char.IsSymbol))
                {
                    context.AddFailure("Password must contain at least one symbol");
                }
            });
        }
    }
}
