using Authentication.Enums;

namespace Authentication.Endpoints.Register
{
    public class RegisterRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required Role Role { get; set; }
    }
}
