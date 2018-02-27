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
            return Ok(getReportEngine);
        }

        //POST api/report
        public IHttpActionResult Post(Report report)
        {
            if (report == null)
            {
                return BadRequest("Report object was null in request");
            }
            setReportEngine.InsertReport(report);
            return Ok();
        }
    }
}
