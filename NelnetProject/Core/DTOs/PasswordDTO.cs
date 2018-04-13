using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs
{
    /// <summary>
    /// A DTO that holds the password information that the database has.
    /// </summary>
    public class PasswordDTO
    {
        /// <summary>
        /// The hashed password with the the salt and pepper integrated in the hashing
        /// </summary>
        [Required]
        public string Hashed { get; set; }

        /// <summary>
        /// The randomly generated salt that was used in the specific user's password generation
        /// </summary>
        [Required]
        public string Salt { get; set; }

        /// <summary>
        /// Type of user that password info belongs to
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            var dTO = obj as PasswordDTO;
            return dTO != null &&
                   Hashed == dTO.Hashed &&
                   Salt == dTO.Salt;
        }

        public override int GetHashCode()
        {
            var hashCode = 1651949353;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hashed);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Salt);
            return hashCode;
        }
    }
}
