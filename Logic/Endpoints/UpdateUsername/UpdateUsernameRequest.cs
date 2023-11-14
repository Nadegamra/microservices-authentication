using FastEndpoints;

namespace Authentication.Endpoints.UpdateUsername
{
    public class UpdateUsernameRequest
    {
        [FromClaim("UserId")]
        public int UserId { get; set; }
        public required string NewUsername { get; set; }
    }
}
