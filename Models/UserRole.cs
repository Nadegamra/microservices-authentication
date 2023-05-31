using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    [PrimaryKey("UserId", "RoleId")]
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
