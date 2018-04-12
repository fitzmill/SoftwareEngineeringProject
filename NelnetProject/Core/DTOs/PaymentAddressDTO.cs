using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs
{
    /// <summary>
    /// A DTO that holds billing name and address information for the card holder associated with PaymentSpring
    /// </summary>
    public class PaymentAddressDTO
    {
        /// <summary>
        /// User's PaymentSpring Customer ID
        /// </summary>
        [Required]
        public string CustomerID { get; set; }

        /// <summary>
        /// Cardholder's First Name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Cardholder's Last Name
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Cardholder's Main Street Address
        /// </summary>
        [Required]
        public string StreetAddress1 { get; set; }

        /// <summary>
        /// Cardholder's Additional Street Address Line (Optional)
        /// </summary>
        public string StreetAddress2 { get; set; }

        /// <summary>
        /// Cardholder's Billing Address City
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// Cardholder's Billing Address State
        /// </summary>
        [Required]
        public string State { get; set; }
        
        /// <summary>
        /// Cardholder's Billing Address Zip
        /// </summary>
        [Required]
        public string Zip { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is PaymentAddressDTO dTO &&
                   CustomerID == dTO.CustomerID &&
                   FirstName == dTO.FirstName &&
                   LastName == dTO.LastName &&
                   StreetAddress1 == dTO.StreetAddress1 &&
                   StreetAddress2 == dTO.StreetAddress2 &&
                   City == dTO.City &&
                   State == dTO.State &&
                   Zip == dTO.Zip;
        }

        public override int GetHashCode()
        {
            var hashCode = -23318427;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CustomerID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StreetAddress1);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StreetAddress2);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(City);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(State);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Zip);
            return hashCode;
        }
    }
}
