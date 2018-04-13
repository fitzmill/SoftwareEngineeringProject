using Accessors;
using Core;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Web.Filters;

namespace Web.Controllers
{
    //this is the route in the url for this set of APIs. So the url would be localhost:port/api/admin
    [RoutePrefix("api/admin")]
    [JwtAuthentication(Roles = new UserType[] { UserType.ADMIN })]
    public class AdminController : ApiController
    {
        IGetTransactionEngine getTransactionEngine;
        IGetReportEngine getReportEngine;
        ISetReportEngine setReportEngine;

        //this object gets injected as a dependency by Unity
        public AdminController(IGetTransactionEngine getTransactionEngine, IGetReportEngine getReportEngine, ISetReportEngine setReportEngine)
        {
            this.getTransactionEngine = getTransactionEngine;
            this.getReportEngine = getReportEngine;
            this.setReportEngine = setReportEngine;
        }

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

            var result = getTransactionEngine.GetTransactionsForDateRange(startDate, endDate);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllReports")]
        public IHttpActionResult GetAllReports()
        {
            return Ok(getReportEngine.GetAllReports());
        }

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

            var report = setReportEngine.InsertReport(startDate, endDate);

            return Ok(report);
        }
    }
}