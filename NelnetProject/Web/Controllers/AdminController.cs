using Core;
using Core.DTOs;
using Core.Interfaces.Engines;
using System;
using System.Web.Http;
using Web.Filters;

namespace Web.Controllers
{
    /// <summary>
    /// Controller for all admin endpoints.
    /// </summary>
    [RoutePrefix("api/admin")]
    [JwtAuthentication(Roles = new UserType[] { UserType.ADMIN })]
    public class AdminController : ApiController
    {
        private readonly ITransactionEngine _transactionEngine;
        private readonly IReportEngine _reportEngine;

        public AdminController(ITransactionEngine transactionEngine, IReportEngine reportEngine)
        {
            _transactionEngine = transactionEngine;
            _reportEngine = reportEngine;
        }

        /// <summary>
        /// Gets all transactions for a given date range.
        /// </summary>
        /// <param name="dateRangeDTO">The date range object</param>
        /// <returns>A collection of transactions with user info</returns>
        [HttpPost]
        [Route("GetTransactionsForDateRange")]
        public IHttpActionResult GetAllTransactionsForDateRange(DateRangeDTO dateRangeDTO)
        {
            if (dateRangeDTO == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }

            var startDate = new DateTime(dateRangeDTO.StartDate.Year, dateRangeDTO.StartDate.Month, dateRangeDTO.StartDate.Day);
            var endDate = new DateTime(dateRangeDTO.EndDate.Year, dateRangeDTO.EndDate.Month, dateRangeDTO.EndDate.Day);

            var result = _transactionEngine.GetTransactionsForDateRange(startDate, endDate);

            return Ok(result);
        }

        /// <summary>
        /// Gets all reports in the database.
        /// </summary>
        /// <returns>A collection of all the reports</returns>
        [HttpGet]
        [Route("GetAllReports")]
        public IHttpActionResult GetAllReports()
        {
            return Ok(_reportEngine.GetAllReports());
        }

        /// <summary>
        /// Inserts a report into the database and returns it.
        /// </summary>
        /// <param name="dateRange">The date range object</param>
        /// <returns>The report</returns>
        [HttpPost]
        [Route("InsertReport")]
        public IHttpActionResult InsertReport(DateRangeDTO dateRange)
        {
            if (dateRange == null || !ModelState.IsValid)
            {
                return BadRequest("Report object was null in request");
            }

            var startDate = new DateTime(dateRange.StartDate.Year, dateRange.StartDate.Month, dateRange.StartDate.Day);
            var endDate = new DateTime(dateRange.EndDate.Year, dateRange.EndDate.Month, dateRange.EndDate.Day);

            var report = _reportEngine.InsertReport(startDate, endDate);

            return Ok(report);
        }
    }
}