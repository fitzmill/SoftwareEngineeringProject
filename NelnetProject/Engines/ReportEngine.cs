using Core.Interfaces.Accessors;
using Core.Interfaces.Engines;
using Core.Models;
using System;
using System.Collections.Generic;

namespace Engines
{
    public class ReportEngine : IReportEngine
    {
        private readonly IReportAccessor _reportAccessor;

        public ReportEngine(IReportAccessor reportAccessor)
        {
            _reportAccessor = reportAccessor;
        }

        public IEnumerable<Report> GetAllReports()
        {
            return _reportAccessor.GetAllReports();
        }

        public Report InsertReport(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date is later than the end date.");
            }

            var report = new Report()
            {
                DateCreated = DateTime.Now,
                StartDate = startDate,
                EndDate = endDate
            };

            _reportAccessor.InsertReport(report);

            return report;
        }
    }
}
