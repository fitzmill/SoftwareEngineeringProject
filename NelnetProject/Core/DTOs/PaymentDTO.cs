using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    /// <summary>
    /// Request data sent to PaymentSpring to charge a user.
    /// </summary>
    public class PaymentDTO
    {
        /// <summary>
        /// PaymentSpring customer id
        /// </summary>
        [Required]
        public string CustomerID { get; set; }

        /// <summary>
        /// Amount charged in cents.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Amount { get; set; }

        /// <summary>
        /// Sets whether PaymentSpring should email the customer after the transaction.
        /// Defaults to false.
        /// </summary>
        public bool SendReceipt { get; set; } = false;
    }
}
