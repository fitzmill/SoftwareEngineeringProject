using System;
using System.Configuration;
using Accessors;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestGetReportAccessor
    {
        IGetReportAccessor getReportAccessor;
        public TestGetReportAccessor()
        {
            this.getReportAccessor = new GetReportAccessor(ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString);
        }

        [TestMethod]
        public void TestGetAllReports()
        {
            var result = getReportAccessor.GetAllReports();

            Assert.IsNotNull(result);
        }
    }
}
