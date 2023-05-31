using System.ComponentModel.DataAnnotations;

namespace Authentication.Endpoints.Login
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberPassword { get; set; }
    }
}
