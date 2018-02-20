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

            Assert.IsTrue(result.Count > 1);
            Assert.AreEqual(result[0].UserID, 1);
        }

        [TestMethod]
        public void TestGetMostRecentTransactionForUser()
        {
            var result = getTransactionAccessor.GetMostRecentTransactionForUser(1);

            Assert.AreEqual(result.UserID, 1);
        }

        [TestMethod]
        public void TestGetAllUnsettledTransactions()
        {
            var result = getTransactionAccessor.GetAllUnsettledTransactions();

            foreach(Transaction t in result)
            {
                Assert.IsFalse(t.ProcessState == ProcessState.FAILED);
                Assert.IsFalse(t.ProcessState == ProcessState.SUCCESSFUL);
            }
        }

        [TestMethod]
        public void TestGetAllTransactionsForDateRange()
        {
            var startDate = new DateTime(2018, 1, 1);
            var endDate = new DateTime(2018, 6, 1);
            var result = getTransactionAccessor.GetTransactionsForDateRange(startDate, endDate);

            foreach(TransactionWithUserInfoDTO t in result)
            {
                Assert.IsTrue(t.DateDue > startDate);
                Assert.IsTrue(t.DateDue < endDate);
            }
        }
    }
}
