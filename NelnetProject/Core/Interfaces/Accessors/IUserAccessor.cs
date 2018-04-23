using System.Collections.Generic;

namespace Core.Interfaces.Accessors
{
    /// <summary>
    /// Interface for performing CRUD operations with the database
    /// </summary>
    public interface IUserAccessor
    {
        /// <summary>
        /// Gets all general active users from the database
        /// </summary>
        /// <returns>A list of all general active users</returns>
        IEnumerable<User> GetAllActiveUsers();

        /// <summary>
        /// Gets a user's info from the database by a User's ID
        /// </summary>
        /// <param name="userID">The user id for which to retrieve information</param>
        /// <returns>A user model filled out with their information from the database</returns>
        User GetUserInfoByID(int userID);

        /// <summary>
        /// Gets a user's info from the database by a user's email
        /// </summary>
        /// <param name="email">The email associated with the information</param>
        /// <returns>A user model filled out with their information from the database</returns>
        User GetUserInfoByEmail(string email);

        /// <summary>
        /// Checks if an email already exists in the database.
        /// </summary>
        /// <param name="email">The email to check for</param>
        /// <returns>True if the email exists in the database, false otherwise</returns>
        bool EmailExists(string email);

        /// <summary>
        /// Gets a user's Payment Spring customerID
        /// </summary>
        /// <param name="userID">The user id associated with the customer id</param>
        /// <returns>The customer id of the user</returns>
        string GetPaymentSpringCustomerID(int userID);

        /// <summary>
        /// Inserts a new user record into the database with the information contained in the user model
        /// </summary>
        /// <param name="user">The user model to insert</param>
        void InsertPersonalInfo(User user);

        /// <summary>
        /// Updates the user record in the database specified by the userID in the user model
        /// </summary>
        /// <param name="user">The user model to update</param>
        void UpdatePersonalInfo(User user);

        /// <summary>
        /// Delete a user record from the database specified by the userID in the user model
        /// </summary>
        /// <param name="userID">The user id associated with the account to be deleted</param>
        void DeletePersonalInfo(int userID);
    }
}
