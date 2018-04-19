namespace Core.Interfaces
{
    /// <summary>
    /// Accessor for setting transaction information
    /// </summary>
    public interface ISetTransactionAccessor
    {
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
