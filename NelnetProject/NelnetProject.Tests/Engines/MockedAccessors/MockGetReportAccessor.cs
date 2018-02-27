using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockGetReportAccessor : IGetReportAccessor
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

        public IList<Report> GetAllReports()
        {
            return MockDB;
        }
    }
}
