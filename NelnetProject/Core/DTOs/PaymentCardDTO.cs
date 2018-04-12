using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs
{
    /// <summary>
    /// DTO used to hold credit card information for PaymentSpring
    /// </summary>
    public class PaymentCardDTO
    {
        /// <summary>
        /// User's Customer ID
        /// </summary>
        [Required]
        public string CustomerID { get; set; }

        /// <summary>
        /// Cardholder's Credit Card
        /// </summary>
        [Range(0, long.MaxValue)]
        public long CardNumber { get; set; }

        /// <summary>
        /// Cardholder's Credit Card Expiration Year
        /// </summary>
        public int ExpirationYear { get; set; }

        /// <summary>
        /// Cardholder's Credit Card Expiration Month
        /// </summary>
        public int ExpirationMonth { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is PaymentCardDTO dTO &&
                   CustomerID == dTO.CustomerID &&
                   CardNumber == dTO.CardNumber &&
                   ExpirationYear == dTO.ExpirationYear &&
                   ExpirationMonth == dTO.ExpirationMonth;
        }

        public override int GetHashCode()
        {
            var hashCode = 920087689;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CustomerID);
            hashCode = hashCode * -1521134295 + CardNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + ExpirationYear.GetHashCode();
            hashCode = hashCode * -1521134295 + ExpirationMonth.GetHashCode();
            return hashCode;
        }
    }
}
