using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs
{
    public class LoginDTO
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Email { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Password { get; set; }
    }
}
