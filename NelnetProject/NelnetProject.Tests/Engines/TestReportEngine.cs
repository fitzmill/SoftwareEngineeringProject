using Core.Interfaces.Accessors;
using Core.Interfaces.Engines;
using Core.Models;
using Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;
using System;
using System.Linq;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestReportEngine
    {
        private readonly IReportEngine _reportEngine;
        private readonly IReportAccessor _reportAccessor;

        public TestReportEngine()
        {
            _reportAccessor = new MockReportAccessor();
            _reportEngine = new ReportEngine(_reportAccessor);
        }

        [TestMethod]
        public void TestGetAllReports()
        {
            var result = _reportEngine.GetAllReports().ToList();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new DateTime(2018, 9, 30), result[0].DateCreated);
            Assert.AreEqual(new DateTime(2018, 1, 1), result[1].StartDate);
            Assert.AreEqual(new DateTime(2017, 9, 30), result[2].EndDate);
        }

        [TestMethod]
        public void TestInsertReport()
        {
            var report = new Report()
            {
                StartDate = new DateTime(2018, 2, 1),
                EndDate = new DateTime(2018, 3, 1)
            };
            var result = _reportEngine.InsertReport(report.StartDate, report.EndDate);

            Assert.IsTrue(result.ReportID > 0);
            Assert.IsNotNull(result.DateCreated);
            Assert.AreEqual(report.StartDate, result.StartDate);
            Assert.AreEqual(report.EndDate, result.EndDate);
        }
    }
}
