using Core.DTOs;
using System;
using System.Collections.Generic;

namespace Core.Interfaces.Accessors
{
    /// <summary>
    /// Accessor for CRUD operations with transactions in the database.
    /// </summary>
    public interface ITransactionAccessor
    {
        /// <summary>
        /// Gets all transactions from the database for a user based on their userID
        /// </summary>
        /// <param name="userID">The user id associated with the desired transaction information</param>
        /// <returns>A collection of transactions for the user</returns>
        IEnumerable<Transaction> GetAllTransactionsForUser(int userID);

        /// <summary>
        /// Gets transactions from the database for all users that were charged in a given date range
        /// </summary>
        /// <param name="startTime">The oldest date allowed for retrieved transactions</param>
        /// <param name="endTime">The newest date allowed for retrieved transactions</param>
        /// <returns>Collection of transactions that occurred in the date range</returns>
        IEnumerable<TransactionWithUserInfoDTO> GetTransactionsForDateRange(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets all transactions that have not been successfully charged
        /// </summary>
        /// <returns>Collection of all RETRYING and NOT_YET_CHARGED transactions</returns>
        IEnumerable<Transaction> GetAllUnsettledTransactions();

        /// <summary>
        /// Gets all transactions marked as FAILED from the database
        /// </summary>
        /// <returns>Collection of all FAILED transactions</returns>
        IEnumerable<Transaction> GetAllFailedTransactions();

        /// <summary>
        /// Adds a transaction to the database
        /// </summary>
        /// <param name="transaction">The transaction to add to the database</param>
        void AddTransaction(Transaction transaction);

        /// <summary>
        /// Updates a transaction that's already in the database
        /// </summary>
        /// <param name="transaction">The information to update the transaction in the database</param>
        void UpdateTransaction(Transaction transaction);
    }
}
