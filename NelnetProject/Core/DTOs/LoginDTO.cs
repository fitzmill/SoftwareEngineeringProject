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
        /// Holds the JwtToken that gets generated on login
        /// </summary>
        [Required]
        public string JwtToken { get; set; }

        /// <summary>
        /// Holds the logged in user's type for page redirection
        /// </summary>
        [Required]
        public UserType UserType { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj as LoginDTO != null &&
                   JwtToken == (obj as LoginDTO).JwtToken &&
                   UserType == (obj as LoginDTO).UserType;
        }

        public override int GetHashCode()
        {
            var hashCode = 244290743;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(JwtToken);
            hashCode = hashCode * -1521134295 + EqualityComparer<UserType>.Default.GetHashCode(UserType);
            return hashCode;
        }
    }
}
