using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface for engine that generates and sends notifications.
    /// </summary>
    public interface INotificationEngine
    {
        /// <summary>
        /// Generates and sends notifications based on the given transactions.
        /// </summary>
        /// <param name="transactions">List of one or more transactions</param>
        void SendTransactionNotifications(List<Transaction> transactions);
    }
}
