using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [ProtectedPersonalData]
        public required string Username { get; set; }
        public required string NormalizedUsername { get; set; }
        [ProtectedPersonalData]
        public required string Email { get; set; }
        public required string NormalizedEmail { get; set; }
        [PersonalData]
        public required bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
