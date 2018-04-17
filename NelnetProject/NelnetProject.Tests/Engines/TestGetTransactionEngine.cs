using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Interfaces;
using NelnetProject.Tests.Engines.MockedAccessors;
using Engines;
using System.Collections.ObjectModel;
using Core;
using Core.DTOs;
using Core.Exceptions;
using System.Collections.Generic;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestGetTransactionEngine
    {
        private List<Transaction> MockDB = new List<Transaction>{
            new Transaction()
            {
                TransactionID = 1,
                UserID = 1,
                AmountCharged = 99.00,
                DateDue = new DateTime(2018, 2, 9),
                DateCharged = new DateTime(2018, 2, 11),
                ProcessState = ProcessState.SUCCESSFUL,
                ReasonFailed = "Card expired"
            },
            new Transaction()
            {
                TransactionID = 2,
                UserID = 2,
                AmountCharged = 64.00,
                DateDue = new DateTime(2018, 2, 9),
                DateCharged = new DateTime(2018, 2, 9),
                ProcessState = ProcessState.SUCCESSFUL
            },
            new Transaction()
            {
                TransactionID = 3,
                UserID = 3,
                AmountCharged = 55.00,
                DateDue = new DateTime(2018, 2, 9),
                DateCharged = null,
                ProcessState = ProcessState.FAILED,
                ReasonFailed = "Insufficient funds"
            },
            new Transaction()
            {
                TransactionID = 4,
                UserID = 1,
                AmountCharged = 108.00,
                DateDue = new DateTime(2018, 3, 11),
                DateCharged = null,
                ProcessState = ProcessState.NOT_YET_CHARGED
            }
        };

        IGetTransactionEngine getTransactionEngine;
        IGetTransactionAccessor getTransactionAccessor;
        public TestGetTransactionEngine()
        {
            getTransactionAccessor = new MockGetTransactionAccessor(MockDB);
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
        public void TestGetAllFailedTransactions()
        {
            var result = getTransactionEngine.GetAllFailedTransactions();

            Assert.AreEqual(1, result.Count);
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "Start date is later than end date.")]
        public void TestGetTransactionsForOutOfOrderDateRange()
        {
            var startDate = new DateTime(2018, 3, 1);
            var endDate = new DateTime(2018, 3, 28);
            var result = getTransactionEngine.GetTransactionsForDateRange(endDate, startDate);
        }
    }
}
