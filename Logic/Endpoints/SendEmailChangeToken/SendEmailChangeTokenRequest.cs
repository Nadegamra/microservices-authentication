using System.ComponentModel.DataAnnotations;
using FastEndpoints;

namespace Authentication.Endpoints.SendEmailChangeToken
{
    public class SendEmailChangeTokenRequest
    {
        [FromClaim("UserId")]
        public int UserId { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
    }
}
