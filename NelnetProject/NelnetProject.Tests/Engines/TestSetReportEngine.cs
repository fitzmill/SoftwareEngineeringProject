using System;
using Core.Interfaces;
using Core.Models;
using Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestSetReportEngine
    {
        ISetReportAccessor setReportAccessor;
        ISetReportEngine setReportEngine;

        public TestSetReportEngine()
        {
            this.setReportAccessor = new MockSetReportAccessor();
            this.setReportEngine = new SetReportEngine(setReportAccessor);
        }

        [TestMethod]
        public void TestInsertReport()
        {
            var report = new Report()
            {
                StartDate = new DateTime(2018, 2, 1),
                EndDate = new DateTime(2018, 3, 1)
            };
            setReportEngine.InsertReport(report);

            Assert.IsTrue(report.ReportID > 0);
            Assert.IsNotNull(report.DateCreated);
        }
    }
}
