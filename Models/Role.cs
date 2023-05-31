using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
