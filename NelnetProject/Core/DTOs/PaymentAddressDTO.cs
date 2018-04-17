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
    }
}
