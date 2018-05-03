using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    /// <summary>
    /// Holds all of the user payment info in one spot when splitting it up is unnecessary
    /// </summary>
    public class UserPaymentInfoDTO
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
        [Range(1, 12)]
        public int ExpirationMonth { get; set; }
        
        /// <summary>
        /// Cardholder's Cred Card Card Type
        /// </summary>
        public string CardType { get; set; }
    }
}
