using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface for engine that charges payments to users.
    /// </summary>
    public interface IPaymentEngine
    {
        /// <summary>
        /// Charges a list of payments and returns the results.
        /// </summary>
        /// <param name="charges">List of unsettled transactions to charge.</param>
        /// <param name="today">Today's date.</param>
        /// <returns>Results of all the charges.</returns>
        IList<Transaction> ChargePayments(List<Transaction> charges, DateTime today);

        /// <summary>
        /// Generates all payments due during the next period and sends them to the database.
        /// </summary>
        /// <param name="today">Today's date.</param>
        /// <returns>List of all upcoming transactions to be sent as notifications.</returns>
        IList<Transaction> GeneratePayments(DateTime today);

        /// <summary>
        /// Gets the date and amount of the next payment for the user based on their userID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="today">Today's date.</param>
        /// <returns></returns>
        Transaction CalculateNextPaymentForUser(int userID, DateTime today);

    }
}
