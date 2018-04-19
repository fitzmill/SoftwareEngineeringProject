using Core.DTOs;
using System.Collections.Generic;

namespace Core.Interfaces.Engines
{
    /// <summary>
    /// Business logic around User object
    /// </summary>
    public interface IUserEngine
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
        IEnumerable<User> GetAllUsers();

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
        /// Inserts a new user record into the database with the information contained in the user model
        /// </summary>
        /// <param name="user">The user model to insert</param>
        /// <param name="password">The password to be hashed and inserted into the database</param>
        void InsertPersonalInfo(User user, string password);

        /// <summary>
        /// Updates the user record in the database specified by the userID in the user model
        /// </summary>
        /// <param name="user">The user model to update</param>
        void UpdatePersonalInfo(User user);

        /// <summary>
        /// Delete a user record from the database specified by the userID in the user model
        /// </summary>
        /// <param name="userID">The ID of the user in the database to delete</param>
        /// <param name="customerID">The ID of the customer in payment spring</param>
        void DeletePersonalInfo(int userID, string customerID);
    }
}
