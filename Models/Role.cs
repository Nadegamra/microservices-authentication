using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string NormalizedName { get; set; }
    }
}
