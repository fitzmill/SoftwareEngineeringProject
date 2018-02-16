using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Core;

namespace Engines
{
    public class GetTransactionEngine : IGetTransactionEngine
    {
        IGetTransactionAccessor getTransactionAccessor;
        public GetTransactionEngine(IGetTransactionAccessor getTransactionAccessor)
        {
            this.getTransactionAccessor = getTransactionAccessor;
        }

        //Calls identical method in IGetTransactionAccessor
        public IList<Transaction> GetAllTransactionsForUser(int userID)
        {
            throw new NotImplementedException();
        }

        //Calls identical method in IGetTransactionAccessor
        public IList<Transaction> GetAllUnsettledTransactions()
        {
            throw new NotImplementedException();
        }

        //Calls identical method in IGetTransactionAccessor
        public Transaction GetMostRecentTransactionForUser(int userID)
        {
            throw new NotImplementedException();
        }

        //Calls identical method in IGetTransactionAccessor
        public IList<Transaction> GetTransactionsForDateRange(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
