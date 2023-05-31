using FastEndpoints;
using FastEndpoints.Security;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Authentication.Endpoints.GetToken
{
    public class LoginRequest: TokenRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberPassword { get; set; }

        [JsonIgnore] public new string UserId { get; set; } = null!;

        [JsonIgnore] public new string RefreshToken { get; set; } = null!;
    }
}
