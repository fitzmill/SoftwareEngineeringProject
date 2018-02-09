using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    /// <summary>
    /// Request data about customer from Payment Spring.
    /// </summary>
    public class CustomerPaymentInfoDTO
    {
        /// <summary>
        /// Last four digits of customer's credit card number from Payment Spring.
        /// </summary>
        public string CardNumberEnding { get; set; }

        /// <summary>
        /// Expiration month of card given from Payment Spring.
        /// </summary>
        public string CardExpirationMonth { get; set; }

        /// <summary>
        /// Expiration year of card given from Payment Spring.
        /// </summary>
        public string CardExpirationYear { get; set; }

        /// <summary>
        /// Gets a customer's card type from Payment Spring.
        /// Defaults to false.
        /// </summary>
        public string CardType { get; set; }
    }
}