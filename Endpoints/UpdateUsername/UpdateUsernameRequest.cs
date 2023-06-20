using FastEndpoints;

namespace Authentication.Endpoints.UpdateUsername
{
    public class UpdateUsernameRequest
    {
        [FromClaim("UserId")]
        public int UserId { get; set; }
        public string NewUsername { get; set; }
    }
}
