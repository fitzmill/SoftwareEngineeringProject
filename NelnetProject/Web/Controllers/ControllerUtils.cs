using System.Linq;
using System.Security.Claims;

namespace Web.Controllers
{
    /// <summary>
    /// Utility class for controllers to get information from HTTP results.
    /// </summary>
    public class ControllerUtils
    {
        /// <summary>
        /// Gets an user ID from an http result.
        /// </summary>
        /// <param name="user">the ClaimsIdentity user</param>
        /// <returns>The userID of the given user</returns>
        public static string httpGetUserID(ClaimsIdentity user)
        {
            string userID = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userID;
        }

        /// <summary>
        /// Gets an email from an http result
        /// </summary>
        /// <param name="user">the ClaimsIdentity user</param>
        /// <returns>The email of the given user</returns>
        public static string httpGetEmail(ClaimsIdentity user)
        {
            string email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return email;
        }
    }
}