using System;
using System.Configuration;
using System.Data.SqlClient;
using Accessors;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestSetTransactionAccessor
    {

        private ISetTransactionAccessor setTransactionAccessor;
        private string connectionString;

        public TestSetTransactionAccessor()
        {
            connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;
            setTransactionAccessor = new SetTransactionAccessor(connectionString);
        }

        [TestMethod]
        public void TestConnectionString()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                con.Close();
            }
        }
    }
}
