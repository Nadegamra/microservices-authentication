﻿using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
    public class EmailConfirmationToken
    {
        [Key]
        public int Id { get; set; }
        public required int UserId { get; set; }
        public required string Token { get; set; }
    }
}
