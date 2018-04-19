using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces
{
    /// <summary>
    /// Methods for retrieving report information
    /// </summary>
    public interface IGetReportAccessor
    {
        /// <summary>
        /// Gets all reports from the database in descending order by executing a stored procedure
        /// </summary>
        /// <returns>List of all reports ordered from most recent to oldest</returns>
        IList<Report> GetAllReports();
    }
}
