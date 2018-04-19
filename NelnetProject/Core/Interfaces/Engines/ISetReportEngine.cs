using Core.Models;
using System;

namespace Core.Interfaces
{
    /// <summary>
    /// Methods for altering report information
    /// </summary>
    public interface ISetReportEngine
    {
        /// <summary>
        /// Creates a report from the given start and end date and inserts it into the database.
        /// </summary>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <returns>The report that was generated</returns>
        Report InsertReport(DateTime startDate, DateTime endDate);
    }
}
