namespace Authentication.Endpoints.Register
{
    public class RegisterRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required int Role { get; set; }
    }
}
