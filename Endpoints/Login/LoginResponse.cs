namespace Authentication.Endpoints.Login
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string? Role { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool? EmailConfirmed { get; set; }
    }
}
