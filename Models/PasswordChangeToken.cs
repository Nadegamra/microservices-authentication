using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class PasswordChangeToken
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
