using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.DTOs;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockGetTransactionAccessor : IGetTransactionAccessor
    {
        private IList<Transaction> MockDB;

        public MockGetTransactionAccessor(IList<Transaction> MockDB)
        {
            this.MockDB = MockDB;
        }

        public IList<Transaction> GetAllTransactionsForUser(int userID)
        {
            return MockDB.Where(x => x.UserID == userID).ToList();
        }

        public IList<Transaction> GetAllUnsettledTransactions()
        {
            return MockDB.Where(x => x.ProcessState != ProcessState.SUCCESSFUL && x.ProcessState != ProcessState.FAILED).ToList();
        }

        public IList<Transaction> GetAllFailedTransactions()
        {
            return MockDB.Where(x => x.ProcessState == ProcessState.FAILED).ToList();
        }

        public IList<TransactionWithUserInfoDTO> GetTransactionsForDateRange(DateTime startTime, DateTime endTime)
        {
            var result = new List<TransactionWithUserInfoDTO>();
            foreach(Transaction t in MockDB.Where(x => x.DateDue >= startTime && x.DateDue <= endTime).ToList())
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
    }
}
