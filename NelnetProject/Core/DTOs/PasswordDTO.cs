using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class PasswordDTO
    {
        public string Hashed { get; set; }

        public string Salt { get; set; }
    }
}
