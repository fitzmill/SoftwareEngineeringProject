using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockSetReportAccessor
    {
        public List<Report> MockDB = new List<Report>()
        {
            new Report()
            {
                ReportID = 1,
                DateCreated = new DateTime(2017, 9, 1),
                StartDate = new DateTime(2017, 8, 1),
                EndDate = new DateTime(2017, 8, 30)
            }
        };

        public void InsertReport(Report report)
        {
            report.ReportID = MockDB.Select(x => x.ReportID).Max() + 1;
            report.DateCreated = DateTime.Now;
            MockDB.Add(report);
        }
    }
}
