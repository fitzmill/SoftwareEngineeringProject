using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engines
{
    public class SetReportEngine : ISetReportEngine
    {
        ISetReportAccessor setReportAccessor;

        public SetReportEngine(ISetReportAccessor setReportAccessor)
        {
            this.setReportAccessor = setReportAccessor;
        }

        public void InsertReport(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ReportException("Start date is later than the end date.");
            }

            var report = new Report()
            {
                DateCreated = DateTime.Now,
                StartDate = startDate,
                EndDate = endDate
            };

            setReportAccessor.InsertReport(report);
        }
    }
}
