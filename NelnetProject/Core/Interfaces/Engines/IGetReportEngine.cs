using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces
{
    /// <summary>
    /// Methods for retrieving report information
    /// </summary>
    public interface IGetReportEngine
    {
        /// <summary>
        /// Gets all reports in the database from the accessor in descending order
        /// </summary>
        /// <returns>List of all reports from most recent to oldest</returns>
        IList<Report> GetAllReports();
    }
}
