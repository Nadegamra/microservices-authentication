using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class UserToken
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; }
        public string AccessToken { get; set; } = null!;
        public DateTime AccessExpiry { get; internal set; }
        public DateTime RefreshExpiry { get; internal set; }
    }
}
