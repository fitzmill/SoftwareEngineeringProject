using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        IGetReportEngine getReportEngine;
        ISetReportEngine setReportEngine;

        public ReportController(IGetReportEngine getReportEngine, ISetReportEngine setReportEngine)
        {
            this.getReportEngine = getReportEngine;
            this.setReportEngine = setReportEngine;
        }

        //GET api/report
        public IHttpActionResult Get()
        {
            return Ok(getReportEngine.GetAllReports());
        }

        //POST api/report
        public IHttpActionResult Post(DateRangeDTO dateRange)
        {
            if (dateRange == null || dateRange.StartDate == null || dateRange.EndDate == null)
            {
                return BadRequest("Report object was null in request");
            }

            var startDate = new DateTime(dateRange.StartDate.Year, dateRange.StartDate.Month, dateRange.StartDate.Day);
            var endDate = new DateTime(dateRange.EndDate.Year, dateRange.EndDate.Month, dateRange.EndDate.Day);
            if (endDate < startDate)
            {
                return BadRequest("Invalid date range");
            }
            
            var report = new Report()
            {
                StartDate = startDate,
                EndDate = endDate
            };
            setReportEngine.InsertReport(report);
            return Ok();
        }
    }
}
