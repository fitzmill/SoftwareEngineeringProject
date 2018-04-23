using Core.DTOs;
using System;
using System.Collections.Generic;

namespace Core.Interfaces.Engines
{
    /// <summary>
    /// Engine for computations relating to transactions.
    /// </summary>
    public interface ITransactionEngine
    {
        /// <summary>
        /// Gets all transactions from the accessor for a user based on their userID
        /// </summary>
        /// <param name="userID">The id of the user associated with the transaction</param>
        /// <returns>A collection of transactions for the user</returns>
        IEnumerable<Transaction> GetAllTransactionsForUser(int userID);

        /// <summary>
        /// Gets transactions from the accesesor for all users that were charged in a given date range
        /// </summary>
        /// <param name="startTime">The start of the date range</param>
        /// <param name="endTime">The end to the date range</param>
        /// <returns>Collection of transactions that occurred in the date range</returns>
        IEnumerable<TransactionWithUserInfoDTO> GetTransactionsForDateRange(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gets all transactions that have not been marked RETRYING or NOT_YET_CHARGED
        /// </summary>
        /// <returns>Collection of all unsettled transactions</returns>
        IEnumerable<Transaction> GetAllUnsettledTransactions();

        /// <summary>
        /// Gets all transactions that have been marked as FAILED
        /// </summary>
        /// <returns>Collection of all failed transactions</returns>
        IEnumerable<Transaction> GetAllFailedTransactions();
    }
}
