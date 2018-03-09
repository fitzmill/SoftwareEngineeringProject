using System;
using System.Collections.Generic;

namespace Core
{
    public class User
    {
        public int UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Hashed { get; set; }

        public string Salt { get; set; }

        public PaymentPlan Plan { get; set; }

        public UserType UserType { get; set; }

        public string CustomerID { get; set; }

        public List<Student> Students { get; set; }
    }
}
