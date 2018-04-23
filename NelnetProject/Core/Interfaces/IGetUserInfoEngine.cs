using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

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
        /// <param name="email"></param>
        /// <returns>Returns true or false depending on whether or not an email is already in the database.</returns>
        bool EmailExists(string email);

        /// <summary>
        /// Gets all of the users in the database.
        /// </summary>
        /// <returns>Returns a list of all users in the database.</returns>
        IList<User> GetAllUsers();

        /// <summary>
        /// Validates the login of a user given an email and a password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Returns true or false depending on whether or not the email and password match an account.</returns>
        User ValidateLoginInfo(string email, string password);

        /// <summary>
        /// Gets a user's information when given a user's ID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>Returns a User that has all of the user's information.</returns>
        User GetUserInfoByID(int userID);

        /// <summary>
        /// Gets a user's payment info from Payment Spring with their customer ID.
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns>A user's payment info provided by Payment Spring/returns>
        UserPaymentInfoDTO GetPaymentInfoForUser(int userID);
    }
}
