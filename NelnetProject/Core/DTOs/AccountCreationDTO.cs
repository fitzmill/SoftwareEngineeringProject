using System.Collections.Generic;

namespace Core.DTOs
{
    /// <summary>
    /// Used for sending the user information from the web when the user creates a new account
    /// </summary>
    public class AccountCreationDTO
    {
        /// <summary>
        /// The first name of the user associated with the account
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user associated with the account
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email to be associated with the user's account, also used for logging in
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's password used for logging in
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The payment plan selected by the user, either monthly, semesterly, or yearly
        /// </summary>
        public PaymentPlan Plan { get; set; }

        /// <summary>
        /// The user type for the account being created
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// All of the students to be associated with the user's account
        /// </summary>
        public List<Student> Students { get; set; }

        /// <summary>
        /// The first name of the cardholder
        /// </summary>
        public string CardholderFirstName { get; set; }

        /// <summary>
        /// The last name of the cardholder
        /// </summary>
        public string CardholderLastName { get; set; }

        /// <summary>
        /// The first address line of the billing address
        /// </summary>
        public string StreetAddress1 { get; set; }

        /// <summary>
        /// The second address line of the billing address
        /// </summary>
        public string StreetAddress2 { get; set; }

        /// <summary>
        /// The name of the city in the billing address
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The state associated with the billing address
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The zip code of the billing address
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// The credit card number associated with the account
        /// </summary>
        public long CardNumber { get; set; }

        /// <summary>
        /// The expiration year of the credit card
        /// </summary>
        public int ExpirationYear { get; set; }

        /// <summary>
        /// The expiration month of the credit card
        /// </summary>
        public int ExpirationMonth { get; set; }
    }
}
