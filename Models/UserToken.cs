using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class UserToken
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string RefreshTokenHash { get; set; }
        public DateTime AccessExpiry { get; internal set; }
        public DateTime RefreshExpiry { get; internal set; }
        public bool Used { get; set; } = false;
    }
}
