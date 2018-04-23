using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTOs;
using Core.Interfaces.Accessors;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockTransactionAccessor : ITransactionAccessor
    {
        public IList<Transaction> _transactions { get; set; } = new List<Transaction>();

        public MockTransactionAccessor(IList<Transaction> MockDB)
        {
            _transactions = MockDB;
        }

        public IEnumerable<Transaction> GetAllTransactionsForUser(int userID)
        {
            return _transactions.Where(x => x.UserID == userID).ToList();
        }

        public IEnumerable<Transaction> GetAllUnsettledTransactions()
        {
            return _transactions.Where(x => x.ProcessState != ProcessState.SUCCESSFUL && x.ProcessState != ProcessState.FAILED).ToList();
        }

        public IEnumerable<Transaction> GetAllFailedTransactions()
        {
            return _transactions.Where(x => x.ProcessState == ProcessState.FAILED).ToList();
        }

        public IEnumerable<TransactionWithUserInfoDTO> GetTransactionsForDateRange(DateTime startTime, DateTime endTime)
        {
            var result = new List<TransactionWithUserInfoDTO>();
            foreach(Transaction t in _transactions.Where(x => x.DateDue >= startTime && x.DateDue <= endTime).ToList())
            {
                result.Add(new TransactionWithUserInfoDTO()
                {
                    TransactionID = t.TransactionID,
                    FirstName = "Bob",
                    LastName = "Smith",
                    AmountCharged = t.AmountCharged,
                    DateDue = t.DateDue,
                    DateCharged = t.DateCharged,
                    ProcessState = t.ProcessState.ToString(),
                    ReasonFailed = t.ReasonFailed
                });
            }
            return result;
        }

        public void AddTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            _transactions.Remove(_transactions.ToList().Find(t => t.TransactionID == transaction.TransactionID));
            _transactions.Add(transaction);
        }
    }
}
