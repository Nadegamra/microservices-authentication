namespace Authentication.Endpoints.Profile
{
    public class ProfileResponse
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required bool EmailConfirmed { get; set; }
    }
}
