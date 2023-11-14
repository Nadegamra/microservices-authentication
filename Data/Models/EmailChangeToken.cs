using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class EmailChangeToken
    {
        [Key]
        public int Id { get; set; }
        public required int UserId { get; set; }
        public string Token { get; set; }
        public required string EmailAddress { get; set; }
    }
}
