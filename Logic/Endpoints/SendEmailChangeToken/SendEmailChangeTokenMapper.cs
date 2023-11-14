using Authentication.Models;
using FastEndpoints;

namespace Authentication.Endpoints.SendEmailChangeToken
{
    public class SendEmailChangeTokenMapper: RequestMapper<SendEmailChangeTokenRequest, EmailChangeToken>
    {
        public override EmailChangeToken ToEntity(SendEmailChangeTokenRequest r)
        {
            return new EmailChangeToken
            {
                EmailAddress = r.EmailAddress,
                UserId = r.UserId,
            };
        }
    }
}
