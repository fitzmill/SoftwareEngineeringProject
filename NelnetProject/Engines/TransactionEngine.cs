using Core;
using Core.DTOs;
using Core.Interfaces.Accessors;
using Core.Interfaces.Engines;
using Engines.Utils;
using System;
using System.Collections.Generic;

namespace Engines
{
    public class TransactionEngine : ITransactionEngine
    {
        private readonly ITransactionAccessor _transactionAccessor;

        public TransactionEngine(ITransactionAccessor transactionAccessor)
        {
            _transactionAccessor = transactionAccessor;
        }
        public IEnumerable<Transaction> GetAllTransactionsForUser(int userID)
        {
            EngineArgumentValidation.ArgumentIsNonNegative(userID, "UserID");
            return _transactionAccessor.GetAllTransactionsForUser(userID);
        }

        public IEnumerable<Transaction> GetAllUnsettledTransactions()
        {
            return _transactionAccessor.GetAllUnsettledTransactions();
        }

        public IEnumerable<Transaction> GetAllFailedTransactions()
        {
            return _transactionAccessor.GetAllFailedTransactions();
        }

        public IEnumerable<TransactionWithUserInfoDTO> GetTransactionsForDateRange(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime)
            {
                throw new ArgumentException("Start date is later than end date.");
            }
            return _transactionAccessor.GetTransactionsForDateRange(startTime, endTime);
        }
    }
}
