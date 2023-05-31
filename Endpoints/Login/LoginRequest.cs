using FastEndpoints;
using FastEndpoints.Security;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Authentication.Endpoints.Login
{
    public class LoginRequest: TokenRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberPassword { get; set; }
    }
}
