using Authentication.Models;
using FastEndpoints;

namespace Authentication.Endpoints.Register
{
    public class RegisterMapper: RequestMapper<RegisterRequest,User>
    {
        public override User ToEntity(RegisterRequest r)
        {
            return new User
            {
                Email = r.Email,
                NormalizedEmail = r.Email.ToUpper(),
                EmailConfirmed = false,
                Username = r.Email,
                NormalizedUsername = r.Email.ToUpper(),
            };
        }
    }
}
