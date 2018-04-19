using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Engines
{
    /// <summary>
    /// Engine for computations relating to reports.
    /// </summary>
    interface IReportEngine
    {
        /// <summary>
        /// Gets all reports in the database from the accessor in descending order
        /// </summary>
        /// <returns>List of all reports from most recent to oldest</returns>
        IList<Report> GetAllReports();

        /// <summary>
        /// Creates a report from the given start and end date and inserts it into the database.
        /// </summary>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <returns>The report that was generated</returns>
        Report InsertReport(DateTime startDate, DateTime endDate);
    }
}
