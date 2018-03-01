using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Interfaces;
using NelnetProject.Tests.Engines.MockedAccessors;
using Engines;
using System.Collections.ObjectModel;
using Core;
using Core.DTOs;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestGetTransactionEngine
    {
        IGetTransactionEngine getTransactionEngine;
        IGetTransactionAccessor getTransactionAccessor;
        public TestGetTransactionEngine()
        {
            getTransactionAccessor = new MockTransactionAccessor();
            getTransactionEngine = new GetTransactionEngine(getTransactionAccessor);
        }

        [TestMethod]
        public void TestGetAllUnsettledTransactions()
        {
            var result = getTransactionEngine.GetAllUnsettledTransactions();
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(Transaction));
            Assert.AreEqual(4, result[0].TransactionID);
        }

        [TestMethod]
        public void TestGetAllTransactionsForUser()
        {
            var id = 1;
            var result = getTransactionEngine.GetAllTransactionsForUser(id);

            Assert.AreEqual(2, result.Count);
            foreach (Transaction t in result)
            {
                Assert.IsInstanceOfType(t, typeof(Transaction));
                Assert.AreEqual(id, t.UserID);
            }
        }

        [TestMethod]
        public void TestGetAllTransactionsForNonexistantUser()
        {
            var id = -1;
            var result = getTransactionEngine.GetAllTransactionsForUser(id);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestGetMostRecentTransactionForUser()
        {
            var id = 1;
            var result = getTransactionEngine.GetMostRecentTransactionForUser(id);

            Assert.AreEqual(new DateTime(2018, 3, 11), result.DateDue);
            Assert.AreEqual(id, result.UserID);
        }

        [TestMethod]
        public void TestGetMostRecentTransactionForNonexistantUser()
        {
            var id = -1;
            var result = getTransactionEngine.GetMostRecentTransactionForUser(id);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetTransactionsForDateRange1()
        {
            var startDate = new DateTime(2018, 2, 1);
            var endDate = new DateTime(2018, 2, 28);
            var result = getTransactionEngine.GetTransactionsForDateRange(startDate, endDate);

            Assert.AreEqual(3, result.Count);
            foreach(TransactionWithUserInfoDTO t in result)
            {
                Assert.IsInstanceOfType(t, typeof(TransactionWithUserInfoDTO));
                Assert.IsTrue(t.DateDue > startDate);
                Assert.IsTrue(t.DateDue < endDate);
            }
        }

        [TestMethod]
        public void TestGetTransactionsForDateRange2()
        {
            var startDate = new DateTime(2018, 3, 1);
            var endDate = new DateTime(2018, 3, 28);
            var result = getTransactionEngine.GetTransactionsForDateRange(startDate, endDate);

            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(TransactionWithUserInfoDTO));
            Assert.IsTrue(result[0].DateDue > startDate);
            Assert.IsTrue(result[0].DateDue < endDate);
            
        }
    }
}
