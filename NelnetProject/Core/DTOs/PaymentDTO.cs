using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            var dTO = obj as PaymentDTO;
            return dTO != null &&
                   CustomerID == dTO.CustomerID &&
                   Amount == dTO.Amount &&
                   SendReceipt == dTO.SendReceipt;
        }

        public override int GetHashCode()
        {
            var hashCode = -622480978;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CustomerID);
            hashCode = hashCode * -1521134295 + Amount.GetHashCode();
            hashCode = hashCode * -1521134295 + SendReceipt.GetHashCode();
            return hashCode;
        }
    }
}
