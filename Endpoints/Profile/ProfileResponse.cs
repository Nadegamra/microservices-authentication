using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Endpoints.Profile
{
    public class ProfileResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
