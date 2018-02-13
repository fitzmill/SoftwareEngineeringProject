using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class UserPaymentInfoDTO
    {
        public string CustomerID { get; set; }

        public string Username { get; set; }

        public string Company { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public int CardNumber { get; set; }

        public int ExpirationYear { get; set; }

        public int ExpirationMonth { get; set; }


    }
}
