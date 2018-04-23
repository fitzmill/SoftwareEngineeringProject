using Core.Interfaces.Accessors;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockReportAccessor : IReportAccessor
    {
        private List<Report> MockDB = new List<Report>()
        {
            new Report()
            {
                ReportID = 3,
                DateCreated = new DateTime(2018, 9, 30),
                StartDate = new DateTime(2018, 9, 1),
                EndDate = new DateTime(2018, 9, 30)
            },
            new Report()
            {
                ReportID = 2,
                DateCreated = new DateTime(2018, 2, 1),
                StartDate = new DateTime(2018, 1, 1),
                EndDate = new DateTime(2018, 2, 28)
            },
            new Report()
            {
                ReportID = 1,
                DateCreated = new DateTime(2017, 11, 1),
                StartDate = new DateTime(2017, 9, 1),
                EndDate = new DateTime(2017, 9, 30)
            }
        };

        public IEnumerable<Report> GetAllReports()
        {
            return MockDB;
        }

        public void InsertReport(Report report)
        {
            report.ReportID = MockDB.Select(x => x.ReportID).Max() + 1;
            report.DateCreated = DateTime.Now;
            MockDB.Add(report);
        }
    }
}
