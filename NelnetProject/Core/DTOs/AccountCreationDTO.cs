using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    /// <summary>
    /// Used for sending the user information from the web when the user creates a new account
    /// </summary>
    public class AccountCreationDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public PaymentPlan Plan { get; set; }

        public UserType UserType { get; set; }

        public List<Student> Students { get; set; }

        public string CardholderFirstName { get; set; }

        public string CardholderLastName { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public long CardNumber { get; set; }

        public int ExpirationYear { get; set; }

        public int ExpirationMonth { get; set; }
    }
}
