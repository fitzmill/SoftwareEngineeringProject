using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IGetUserInfoAccessor
    {
        /// <summary>
        /// Gets a user's info from the database by a User's ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>A user's info</returns>
        User GetUserInfoByID(int userID);

        /// <summary>
        /// Gets a user's info from the database by a user's email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A user's info</returns>
        User GetUserInfoByEmail(string email);

        /// <summary>
        /// Gets a user's password information for logging in
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A user's password hashed plus the salt data</returns>
        PasswordDTO GetUserPasswordInfo(string email);

        /// <summary>
        /// Checks if an email already exists in the database.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True if the email exists in the database, false otherwise</returns>
        bool EmailExists(string email);

        /// <summary>
        /// Gets a user's Payment Spring customerID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>Customer's customerID</returns>
        int GetPaymentSpringCustomerID(int userID);
    }
}
