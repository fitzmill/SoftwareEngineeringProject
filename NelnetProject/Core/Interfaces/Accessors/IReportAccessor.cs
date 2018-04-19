using Core.Models;
using System.Collections.Generic;

namespace Core.Interfaces.Accessors
{
    /// <summary>
    /// Accessor for CRUD operations with reports in the database.
    /// </summary>
    interface IReportAccessor
    {
        /// <summary>
        /// Gets all reports from the database in descending order by executing a stored procedure
        /// </summary>
        /// <returns>List of all reports ordered from most recent to oldest</returns>
        IList<Report> GetAllReports();

        /// <summary>
        /// Inserts report into database by calling stored procedure
        /// </summary>
        /// <param name="report">Report to be inserted</param>
        void InsertReport(Report report);
    }
}
