using Authentication.Models;
using FastEndpoints;

namespace Authentication.Endpoints.Profile
{
    public class ProfileMapper : ResponseMapper<ProfileResponse, User>
    {
        public override ProfileResponse FromEntity(User e)
        {
            return new ProfileResponse
            {
                Id = e.Id,
                Email = e.Email,
                EmailConfirmed = e.EmailConfirmed,
                Username = e.Username,
            };
        }
    }
}
