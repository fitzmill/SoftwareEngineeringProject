using System;
using System.Configuration;
using System.Data.SqlClient;
using Accessors;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestSetReportAccessor
    {
        ISetReportAccessor setReportAccessor;
        string connectionString;

        public TestSetReportAccessor()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;
            setReportAccessor = new SetReportAccessor(connectionString);
        }

        public object ConfigrationManager { get; }

        [TestMethod]
        public void TestConnectionString()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                conn.Close();
            }
        }
    }
}
