using System;
using Core.Interfaces;
using Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestGetReportEngine
    {
        IGetReportEngine getReportEngine;
        IGetReportAccessor getReportAccessor;

        public TestGetReportEngine()
        {
            this.getReportAccessor = new MockGetReportAccessor();
            this.getReportEngine = new GetReportEngine(getReportAccessor);
        }

        [TestMethod]
        public void TestGetAllReports()
        {
            var result = getReportEngine.GetAllReports();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new DateTime(2018, 9, 30), result[0].DateCreated);
            Assert.AreEqual(new DateTime(2018, 1, 1), result[1].StartDate);
            Assert.AreEqual(new DateTime(2017, 9, 30), result[2].EndDate);
        }
    }
}
