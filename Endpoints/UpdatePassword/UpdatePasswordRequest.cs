using FastEndpoints;

namespace Authentication.Endpoints.UpdatePassword
{
    public class UpdatePasswordRequest
    {
        [FromClaim("UserId")]
        public int UserId { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
