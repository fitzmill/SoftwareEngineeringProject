using System.Configuration;
using System.Data.SqlClient;
using Accessors;
using Core.Interfaces.Accessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestReportAccessor
    {
        private readonly string _connectionString;
        private readonly IReportAccessor _reportAccessor;

        public TestReportAccessor()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;
            _reportAccessor = new ReportAccessor(_connectionString);
        }

        [TestMethod]
        public void TestGetAllReports()
        {
            var result = _reportAccessor.GetAllReports();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestConnectionString()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                conn.Close();
            }
        }
    }
}
