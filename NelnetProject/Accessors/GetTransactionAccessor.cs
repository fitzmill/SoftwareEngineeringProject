using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Core;

namespace Accessors
{
    public class GetTransactionAccessor : IGetTransactionAccessor
    {
        public GetTransactionAccessor()
        {

        }

        //Executes a stored procedure in the database for getting all Transactions with userID as a parameter
        public List<Transaction> GetAllTransactionsForUser(int userID)
        {
            throw new NotImplementedException();
        }

        //Executes a stored procedure in the database for getting all Transactions with ProcessState as a parameter
        public List<Transaction> GetAllUnsettledTransactions()
        {
            throw new NotImplementedException();
        }

        //Executes a stored procedure in the database for getting the most recent transaction with userID as a parameter
        public Transaction GetMostRecentTransactionForUser(int userID)
        {
            throw new NotImplementedException();
        }

        //Executes a stored procedure in the database for getting all Transactions with startTime and endTime as parameters
        public List<Transaction> GetTransactionsForDateRange(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
