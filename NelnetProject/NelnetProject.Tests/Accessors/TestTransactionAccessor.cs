using Accessors;
using Core.Interfaces.Accessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestTransactionAccessor
    {
        private readonly ITransactionAccessor _transactionAccessor;

        public TestTransactionAccessor()
        {
            _transactionAccessor = new TransactionAccessor();
        }

        [TestMethod]
        public void TestGetAllTransactionsForUser()
        {
            var result = _transactionAccessor.GetAllTransactionsForUser(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetMostRecentTransactionForUser()
        {
            var result = _transactionAccessor.GetAllFailedTransactions();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetAllUnsettledTransactions()
        {
            var result = _transactionAccessor.GetAllUnsettledTransactions();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetAllTransactionsForDateRange()
        {
            var startDate = new DateTime(2018, 1, 1);
            var endDate = new DateTime(2018, 6, 1);
            var result = _transactionAccessor.GetTransactionsForDateRange(startDate, endDate);

            Assert.IsNotNull(result);
        }
    }
}
