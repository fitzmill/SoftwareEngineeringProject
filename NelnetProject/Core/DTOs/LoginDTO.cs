using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs
{
    /// <summary>
    /// A DTO used to hold user login information
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Holds the email for the login information
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Email { get; set; }

        /// <summary>
        /// Holds the password for the login information
        /// </summary>
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1)]
        public string Password { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            var dTO = obj as LoginDTO;
            return dTO != null &&
                   Email == dTO.Email &&
                   Password == dTO.Password;
        }

        public override int GetHashCode()
        {
            var hashCode = 244290743;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            return hashCode;
        }
    }
}
