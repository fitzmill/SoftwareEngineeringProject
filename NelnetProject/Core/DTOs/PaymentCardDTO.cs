using System.ComponentModel.DataAnnotations;

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
    }
}
