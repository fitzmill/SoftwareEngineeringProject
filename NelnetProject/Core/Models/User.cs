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
        [StringLength(int.MaxValue, MinimumLength = 0)]
        public string Hashed { get; set; }

        /// <summary>
        /// Salt Used When Hashing User's Password
        /// </summary>
        [StringLength(int.MaxValue, MinimumLength = 0)]
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
        [StringLength(int.MaxValue, MinimumLength = 0)]
        public string CustomerID { get; set; }

        /// <summary>
        /// List Of User's Students
        /// </summary>
        [Required]
        public IEnumerable<Student> Students { get; set; }
    }
}
