using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    /// <summary>
    /// Model for the users
    /// </summary>
    public class User
    {
        /// <summary>
        /// User's ID
        /// </summary>
        [Range(0, int.MaxValue)]
        public int UserID { get; set; }

        /// <summary>
        /// User's First Name
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string FirstName { get; set; }

        /// <summary>
        /// User's Last Name
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string LastName { get; set; }

        /// <summary>
        /// User's Email
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Email { get; set; }

        /// <summary>
        /// User's Hash Password With Salt And Pepper
        /// </summary>
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Hashed { get; set; }

        /// <summary>
        /// Salt Used When Hashing User's Password
        /// </summary>
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Salt { get; set; }

        /// <summary>
        /// User's Payment Plan (MONTHLY, SEMESTERLY, YEARLY)
        /// </summary>
        public PaymentPlan Plan { get; set; }

        /// <summary>
        /// User's User Type (GENERAL, ADMIN)
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// User's PaymentSpring Customer ID
        /// </summary>
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string CustomerID { get; set; }

        /// <summary>
        /// List Of User's Students
        /// </summary>
        [Required]
        public List<Student> Students { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
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
