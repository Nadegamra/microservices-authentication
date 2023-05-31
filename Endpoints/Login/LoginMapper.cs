using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Endpoints.Login
{
    public class LoginMapper: ResponseMapper<LoginResponse, IdentityUser<int>>
    {
        public override LoginResponse FromEntity(IdentityUser<int> e)
        {
            return new LoginResponse
            {
                Email = e.Email,
                EmailConfirmed = e.EmailConfirmed,
                Id = e.Id,
                Username = e.UserName,
            };
        }
    }
}
