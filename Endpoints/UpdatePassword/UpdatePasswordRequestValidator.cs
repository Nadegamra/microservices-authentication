using FastEndpoints;
using FluentValidation;

namespace Authentication.Endpoints.UpdatePassword
{
    public class UpdatePasswordRequestValidator: Validator<UpdatePasswordRequest>
    {
        public UpdatePasswordRequestValidator()
        {
            RuleFor(x => x.OldPassword).MinimumLength(8);
            RuleFor(x => x.NewPassword).MinimumLength(8);
        }
    }
}
