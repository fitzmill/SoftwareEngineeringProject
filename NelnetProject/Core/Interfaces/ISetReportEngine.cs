using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    /// <summary>
    /// Methods for altering report information
    /// </summary>
    public interface ISetReportEngine
    {
        /// <summary>
        /// Inserts a report into the database by calling the same method in the ISetReportAccessor
        /// </summary>
        /// <param name="report">Report to be inserted</param>
        void InsertReport(Report report);
    }
}
