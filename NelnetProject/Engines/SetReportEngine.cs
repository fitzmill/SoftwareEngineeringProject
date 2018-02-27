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

        public void InsertReport(Report report)
        {
            setReportAccessor.InsertReport(report);
        }
    }
}
