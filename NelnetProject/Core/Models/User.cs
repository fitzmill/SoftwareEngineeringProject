using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class User
    {
        [Range(1, int.MaxValue)]
        public int UserID { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Email { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Hashed { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Salt { get; set; }

        public PaymentPlan Plan { get; set; }

        public UserType UserType { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string CustomerID { get; set; }

        [Required]
        public List<Student> Students { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   UserID == user.UserID &&
                   FirstName == user.FirstName &&
                   LastName == user.LastName &&
                   Email == user.Email &&
                   Hashed == user.Hashed &&
                   Salt == user.Salt &&
                   Plan == user.Plan &&
                   UserType == user.UserType &&
                   CustomerID == user.CustomerID &&
                   EqualityComparer<List<Student>>.Default.Equals(Students, user.Students);
        }

        public override int GetHashCode()
        {
            var hashCode = 1025807708;
            hashCode = hashCode * -1521134295 + UserID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hashed);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Salt);
            hashCode = hashCode * -1521134295 + Plan.GetHashCode();
            hashCode = hashCode * -1521134295 + UserType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CustomerID);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Student>>.Default.GetHashCode(Students);
            return hashCode;
        }
    }
}
