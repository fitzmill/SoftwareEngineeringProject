using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Methods for altering report information in the database
    /// </summary>
    public interface ISetReportAccessor
    {
        /// <summary>
        /// Inserts report into database by calling stored procedure
        /// </summary>
        /// <param name="report">Report to be inserted</param>
        void InsertReport(Report report);
    }
}
