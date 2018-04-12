using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs
{
    public class UserPaymentInfoDTO
    {
        [Required]
        public string CustomerID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        [Range(0, long.MaxValue)]
        public long CardNumber { get; set; }

        public int ExpirationYear { get; set; }

        [Range(1, 12)]
        public int ExpirationMonth { get; set; }

        public string CardType { get; set; }

        //auto-generated overide to the .Equals method to compare these objects
        public override bool Equals(object obj)
        {
            return obj is UserPaymentInfoDTO dTO &&
                   CustomerID == dTO.CustomerID &&
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

        public override int GetHashCode()
        {
            var hashCode = -1368153767;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CustomerID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StreetAddress1);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StreetAddress2);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(City);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(State);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Zip);
            hashCode = hashCode * -1521134295 + CardNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + ExpirationYear.GetHashCode();
            hashCode = hashCode * -1521134295 + ExpirationMonth.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CardType);
            return hashCode;
        }
    }
}
