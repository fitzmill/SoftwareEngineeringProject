using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Interfaces;
using Accessors;
using System.Configuration;
using Core;
using Core.DTOs;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestGetTransactionAccessor
    {
        IGetTransactionAccessor getTransactionAccessor;
        public TestGetTransactionAccessor()
        {
            this.getTransactionAccessor = new GetTransactionAccessor(ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString);
        }

        [TestMethod]
        public void TestGetAllTransactionsForUser()
        {
            var result = getTransactionAccessor.GetAllTransactionsForUser(1);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetMostRecentTransactionForUser()
        {
            var result = getTransactionAccessor.GetAllFailedTransactions();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetAllUnsettledTransactions()
        {
            var result = getTransactionAccessor.GetAllUnsettledTransactions();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetAllTransactionsForDateRange()
        {
            var startDate = new DateTime(2018, 1, 1);
            var endDate = new DateTime(2018, 6, 1);
            var result = getTransactionAccessor.GetTransactionsForDateRange(startDate, endDate);

            Assert.IsNotNull(result);
        }
    }
}
