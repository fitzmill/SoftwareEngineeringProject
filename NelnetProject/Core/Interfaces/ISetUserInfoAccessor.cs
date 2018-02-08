using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Stores information about users in the database.
    /// </summary>
    interface ISetUserInfoAccessor
    {
        /// <summary>
        /// Creates a user record in the database
        /// </summary>
        /// <param name="user">The user to store in the database</param>
        /// <returns>The id of the newly created user</returns>
        int CreateUser(User user);

        /// <summary>
        /// Updates a user record in the database
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(User user);

    }
}
