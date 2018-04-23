using System.ComponentModel.DataAnnotations;

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
    }
}
