﻿using System.ComponentModel.DataAnnotations;

namespace RestauracjaApi2.Models
{
    public class RegisterUserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }

        public int RoleId { get; set; } = 7;

    }
}
