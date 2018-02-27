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
    public interface IGetReportEngine
    {
        /// <summary>
        /// Gets all reports in the database from the accessor in descending order
        /// </summary>
        /// <returns>List of all reports in descending order</returns>
        IList<Report> GetAllReports();
    }
}
