using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class CreateTokenDTO
    {
        public string Username { get; set; }

        public string TokenType { get; set; }

        public string CardOwnerName { get; set; }

        public int CardNumber { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public int CSC { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }
    }
}
