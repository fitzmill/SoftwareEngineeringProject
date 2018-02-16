using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Interfaces;
using NelnetProject.Tests.Engines.MockedAccessors;
using Engines;

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
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(getTransactionAccessor.GetAllUnsettledTransactions(), result);
        }
    }
}
