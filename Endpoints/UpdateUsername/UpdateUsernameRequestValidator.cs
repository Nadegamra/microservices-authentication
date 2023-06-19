using FastEndpoints;
using FluentValidation;

namespace Authentication.Endpoints.UpdateUsername
{
    public class UpdateUsernameRequestValidator: Validator<UpdateUsernameRequest>
    {
        public UpdateUsernameRequestValidator()
        {
            RuleFor(x => x.NewUsername).MinimumLength(5);
        }
    }
}
