using FastEndpoints;

namespace Authentication.Endpoints.Profile
{
    public class ProfileRequest
    {
        [FromClaim("UserId")]
        public int UserId { get; set; }
    }
}
