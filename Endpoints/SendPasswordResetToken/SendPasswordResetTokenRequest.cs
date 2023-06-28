namespace Authentication.Endpoints.SendPasswordResetToken
{
    public class SendPasswordResetTokenRequest
    {
        public required string EmailAddress { get; set; }
    }
}
