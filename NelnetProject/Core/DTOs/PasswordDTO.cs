using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs
{
    public class PasswordDTO
    {
        [Required]
        public string Hashed { get; set; }

        [Required]
        public string Salt { get; set; }
    }
}
