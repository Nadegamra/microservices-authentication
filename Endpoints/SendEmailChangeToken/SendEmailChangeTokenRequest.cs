using FastEndpoints;

namespace Authentication.Endpoints.SendEmailChangeToken
{
    public class SendEmailChangeTokenRequest
    {
        [FromClaim("UserId")]
        public int UserId { get; set; }
        public string EmailAddress { get; set; }
    }
}
