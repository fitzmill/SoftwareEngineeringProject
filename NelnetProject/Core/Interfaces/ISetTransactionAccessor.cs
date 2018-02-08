using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface ISetTransactionAccessor
    {
        /// <summary>
        /// Adds a transaction to the database
        /// </summary>
        /// <param name="transaction"></param>
        void AddTransaction(Transaction transaction);

        /// <summary>
        /// Updates a transaction that's already in the database
        /// </summary>
        /// <param name="transaction"></param>
        void UpdateTransaction(Transaction transaction);


    }
}
