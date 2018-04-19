using Core.DTOs;
using System;
using System.Collections.Generic;

namespace Core.Interfaces.Engines
{
    /// <summary>
    /// Engine for computations relating to transactions.
    /// </summary>
    interface ITransactionEngine
    {
        /// <summary>
        /// Gets a list of all transactions from the accessor for a user based on their userID
        /// </summary>
        /// <param name="userID">The id of the user associated with the transaction</param>
        /// <returns>A list of transactions for the user</returns>
        IList<Transaction> GetAllTransactionsForUser(int userID);

        /// <summary>
        /// Gets a list of transactions from the accesesor for all users that were charged in a given date range
        /// </summary>
        /// <param name="startTime">The start of the date range</param>
        /// <param name="endTime">The end to the date range</param>
        /// <returns>List of transactions that occurred in the date range</returns>
        IList<TransactionWithUserInfoDTO> GetTransactionsForDateRange(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gets all transactions that have not been marked RETRYING or NOT_YET_CHARGED
        /// </summary>
        /// <returns>List of all unsettled transactions</returns>
        IList<Transaction> GetAllUnsettledTransactions();

        /// <summary>
        /// Gets all transactions that have been marked as FAILED
        /// </summary>
        /// <returns>List of all failed transactions</returns>
        IList<Transaction> GetAllFailedTransactions();
    }
}
