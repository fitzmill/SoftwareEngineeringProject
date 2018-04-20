using System;
using System.Configuration;
using System.Data.SqlClient;
using Accessors;
using Core.Interfaces.Accessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestTransactionAccessor
    {
        private readonly string _connectionString;
        private readonly ITransactionAccessor _transactionAccessor;

        public TestTransactionAccessor()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;
            _transactionAccessor = new TransactionAccessor(_connectionString);
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

        [TestMethod]
        public void TestConnectionString()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                con.Close();
            }
        }
    }
}
