namespace Authentication.Endpoints.ChangePassword
{
    public class ChangePasswordRequest
    {
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}
