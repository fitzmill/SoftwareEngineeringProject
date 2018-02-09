using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IGetTransactionEngine
    {
        /// <summary>
        /// Gets a list of all transactions from the accessor for a user based on their userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>A list of transactions for the user</returns>
        List<Transaction> GetTransactionsForUser(int userID);

        /// <summary>
        /// Gets a list of transactions from the accesesor for all users that were charged in a given date range
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns>List of transactions that occurred in the date range</returns>
        List<Transaction> GetTransactionsForDateRange(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gets all transactions that have not been successfully charged
        /// </summary>
        /// <returns>List of all unsettled transactions</returns>
        List<Transaction> GetAllUnsettledTransactions();

        /// <summary>
        /// Gets the most recent transaction for a user from the accessor based on their userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>Most recent transaction for user</returns>
        Transaction GetTransactionByUserID(int userID);
    }
}
