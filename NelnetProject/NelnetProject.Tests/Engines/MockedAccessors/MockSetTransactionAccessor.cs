using Core;
using Core.Interfaces;
using System.Collections.Generic;

namespace NelnetProject.Tests.Engines
{
    internal class MockSetTransactionAccessor : ISetTransactionAccessor
    {
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            Transactions.Remove(Transactions.Find(t => t.TransactionID == transaction.TransactionID));
            Transactions.Add(transaction);
        }
    }
}