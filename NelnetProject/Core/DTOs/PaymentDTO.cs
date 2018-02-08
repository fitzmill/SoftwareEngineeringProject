using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    /// <summary>
    /// Request data sent to PaymentSpring to charge a user.
    /// </summary>
    public class PaymentDTO
    {
        /// <summary>
        /// Application's private key
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// PaymentSpring customer id
        /// </summary>
        public string CustomerID { get; set; }

        /// <summary>
        /// Amount charged in cents.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Sets whether PaymentSpring should email the customer after the transaction.
        /// Defaults to false.
        /// </summary>
        public bool SendReceipt { get; set; } = false;
    }
}
