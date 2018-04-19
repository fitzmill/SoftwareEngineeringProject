using Core.DTOs;
using System.Collections.Generic;

namespace Core.Interfaces
{   
    /// <summary>
    /// Engine for getting information about a user.
    /// </summary>
    public interface IGetUserInfoEngine
    {
        /// <summary>
        /// Checks to see if a given email matches with an email in the database.
        /// </summary>
        /// <param name="email">The email to check the database for</param>
        /// <returns>Returns true if the email exists and is associated with an active account, false otherwise</returns>
        bool EmailExists(string email);

        /// <summary>
        /// Gets all of the active general users in the database.
        /// </summary>
        /// <returns>Returns a list of all general active users in the database.</returns>
        IList<User> GetAllUsers();

        /// <summary>
        /// Validates the login of a user given an email and a password.
        /// </summary>
        /// <param name="email">The email to verify</param>
        /// <param name="password">The password to verify</param>
        /// <returns>The information associated with the email and password if the credentials are valid, null otherwise</returns>
        User ValidateLoginInfo(string email, string password);

        /// <summary>
        /// Gets a user's information when given a user's ID.
        /// </summary>
        /// <param name="userID">The id associated with the desired user information</param>
        /// <returns>Returns a User model that has all of the user's information.</returns>
        User GetUserInfoByID(int userID);

        /// <summary>
        /// Gets a user's payment info from PaymentSpring with their customer ID.
        /// </summary>
        /// <param name="customerID">The customer id associated with the desired payment information</param>
        /// <returns>A user's payment info provided by PaymentSpring</returns>
        UserPaymentInfoDTO GetPaymentInfoForUser(int userID);
    }
}
