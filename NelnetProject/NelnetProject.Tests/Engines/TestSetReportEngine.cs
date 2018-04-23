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
            var result = setReportEngine.InsertReport(report.StartDate, report.EndDate);

            Assert.IsTrue(result.ReportID > 0);
            Assert.IsNotNull(result.DateCreated);
            Assert.AreEqual(report.StartDate, result.StartDate);
            Assert.AreEqual(report.EndDate, result.EndDate);
        }
    }
}
