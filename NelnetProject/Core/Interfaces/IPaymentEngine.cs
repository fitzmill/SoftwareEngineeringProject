using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface for engine that charges payments to users.
    /// </summary>
    interface IPaymentEngine
    {
        /// <summary>
        /// Charges a list of payments and returns the results.
        /// </summary>
        /// <param name="charges">List of unsettled transactions to charge.</param>
        /// <returns>Results of all the charges.</returns>
        List<Transaction> ChargePayments(List<Transaction> charges);

        /// <summary>
        /// Generates all payments due during the next period and sends them to the database.
        /// </summary>
        /// <returns>List of all upcoming transactions to be sent as notifications.</returns>
        List<Transaction> GeneratePayments();
 
    }
}
