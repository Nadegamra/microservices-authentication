using FastEndpoints;

namespace Authentication.Endpoints.DeleteUser
{
    public class DeleteUserRequest
    {
        [FromClaim("UserId")]
        public int UserId { get; set; }
    }
}