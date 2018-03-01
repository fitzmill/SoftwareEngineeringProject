using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <returns>List of all reports in descending order</returns>
        IList<Report> GetAllReports();
    }
}
