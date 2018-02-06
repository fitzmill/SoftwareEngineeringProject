using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Address
    {
        public int AddressID { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public State State { get; set; }

        public string ZipCode { get; set; }
    }
}
