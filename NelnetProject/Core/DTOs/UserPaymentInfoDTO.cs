using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class UserPaymentInfoDTO
    {
        public string CustomerID { get; set; }

        public string Company { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public long CardNumber { get; set; }

        public int ExpirationYear { get; set; }

        public int ExpirationMonth { get; set; }

        public string CardType { get; set; }

        //auto-generated overide to the .Equals method to compare these objects
        public override bool Equals(object obj)
        {
            var dTO = obj as UserPaymentInfoDTO;
            return dTO != null &&
                   CustomerID == dTO.CustomerID &&
                   Company == dTO.Company &&
                   FirstName == dTO.FirstName &&
                   LastName == dTO.LastName &&
                   StreetAddress1 == dTO.StreetAddress1 &&
                   StreetAddress2 == dTO.StreetAddress2 &&
                   City == dTO.City &&
                   State == dTO.State &&
                   Zip == dTO.Zip &&
                   CardNumber == dTO.CardNumber &&
                   ExpirationYear == dTO.ExpirationYear &&
                   ExpirationMonth == dTO.ExpirationMonth &&
                   CardType == dTO.CardType;
        }
    }
}
