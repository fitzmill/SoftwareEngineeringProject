using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IGetTransactionAccessor
    {
        /// <summary>
        /// Gets a list of all transactions from the database for a user based on their userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>A list of transactions for the user</returns>
        List<Transaction> GetTransactionsForUser(int userID);

        /// <summary>
        /// Gets a list of transactions from the database for all users that were charged in a given date range
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns>List of transactions that occurred in the date range</returns>
        List<Transaction> GetTransactionsForDateRange(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gets all uncharged transactions from the database
        /// </summary>
        /// <returns>List of all uncharged transactions</returns>
        List<Transaction> GetAllUnchargedTransactions();

        /// <summary>
        /// Gets the most recent transaction for a user from the database based on their userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>Most recent transaction for user</returns>
        Transaction GetTransactionByUserID(int userID);
    }
}
