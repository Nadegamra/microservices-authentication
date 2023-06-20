using FastEndpoints;
using FluentValidation;

namespace Authentication.Endpoints.DeleteUser
{
    public class DeleteUserRequestValidator : Validator<DeleteUserRequest>
    {
        public DeleteUserRequestValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}