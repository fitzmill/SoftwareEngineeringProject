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
    }
}
