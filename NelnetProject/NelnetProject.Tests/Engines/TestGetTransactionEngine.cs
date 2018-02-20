using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Interfaces;
using NelnetProject.Tests.Engines.MockedAccessors;
using Engines;
using System.Collections.ObjectModel;

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
        public void TestGetAllUnsettledTransactions1()
        {
            var result = getTransactionEngine.GetAllUnsettledTransactions();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(4, result[0].TransactionID);
        }
    }
}
