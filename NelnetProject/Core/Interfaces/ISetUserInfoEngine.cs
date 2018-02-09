using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Handles the storage of all user information
    /// </summary>
    interface ISetUserInfoEngine
    {
        /// <summary>
        /// Creates a user record in the database and an associated customer in paymentSpring
        /// </summary>
        /// <param name="user">The user model to store</param>
        /// <param name="cardNumber">A credit card number</param>
        /// <param name="expirationYear">Year of expiration</param>
        /// <param name="expirationMonth">Month of expiration</param>
        /// <param name="CSC">Security code</param>
        void CreateUser(User user, string cardNumber, string expirationYear, string expirationMonth, string CSC);

        /// <summary>
        /// Updates a user record in the database and the associated customer in paymentSpring
        /// </summary>
        /// <param name="user">The user model to update</param>
        /// <param name="cardNumber">A credit card number</param>
        /// <param name="expirationYear">Year of expiration</param>
        /// <param name="expirationMonth">Month of expiration</param>
        /// <param name="CSC">Security code</param>
        void UpdateUser(User user, string cardNumber, string expirationYear, string expirationMonth, string CSC);
    }
}
