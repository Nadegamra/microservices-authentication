using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [ProtectedPersonalData]
        public string Username { get; set; }
        public string NormalizedUsername { get; set; }
        [ProtectedPersonalData]
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        [PersonalData]
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
