using Microsoft.EntityFrameworkCore;

namespace Authentication.Models
{
    [PrimaryKey("UserId", "RoleId")]
    public class UserRole
    {
        public required int UserId { get; set; }
        public required int RoleId { get; set; }
    }
}
