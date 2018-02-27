using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engines
{
    public class GetReportEngine : IGetReportEngine
    {
        IGetReportAccessor getReportAccessor;

        public GetReportEngine(IGetReportAccessor getReportAccessor)
        {
            this.getReportAccessor = getReportAccessor;
        }

        public IList<Report> GetAllReports()
        {
            return getReportAccessor.GetAllReports();
        }
    }
}
