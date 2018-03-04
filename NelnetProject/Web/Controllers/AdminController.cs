using Accessors;
using Core;
using Core.DTOs;
using Core.Interfaces;
using Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Web.Controllers
{
    //this is the route in the url for this set of APIs. So the url would be localhost:port/api/admin
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        IGetTransactionEngine getTransactionEngine;
        //this object gets injected as a dependency by Unity
        public AdminController(IGetTransactionEngine getTransactionEngine)
        {
            this.getTransactionEngine = getTransactionEngine;
        }

        // GET api/admin/GetAllTransactionsForUser/5
        //This is a get request with the above route. The 5 at the end of the example is an example userID
        [HttpGet]
        [Route("GetAllTransactionsForUser/{userID}")]
        public IHttpActionResult GetAllTransactionsForUser(string userID)
        {
            //Tries to convert the parameter to an int
            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }
            return Ok(getTransactionEngine.GetAllTransactionsForUser(parsedUserID));
        }

        [HttpGet]
        [Route("GetMostRecentTransactionForUser/{userID}")]
        public IHttpActionResult GetMostRecentTransactionForUser(string userID)
        {
            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }
            return Ok(getTransactionEngine.GetMostRecentTransactionForUser(parsedUserID));
        }

        [HttpGet]
        [Route("GetAllUnsettledTransactions")]
        public IHttpActionResult GetAllUnsettledTransactions()
        {
            return Ok(getTransactionEngine.GetAllUnsettledTransactions());
        }

        [HttpPost]
        [Route("GetTransactionsForDateRange")]
        public IHttpActionResult GetAllTransactionsForDateRange(DateRangeDTO dateRangeDTO)
        {
            if (dateRangeDTO == null || dateRangeDTO.StartDate == null || dateRangeDTO.EndDate == null)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            var startDate = new DateTime(dateRangeDTO.StartDate.Year, dateRangeDTO.StartDate.Month, dateRangeDTO.StartDate.Day);
            var endDate = new DateTime(dateRangeDTO.EndDate.Year, dateRangeDTO.EndDate.Month, dateRangeDTO.EndDate.Day);
            if (endDate < startDate)
            {
                return BadRequest("End date is set to before start date");
            }
            return Ok(getTransactionEngine.GetTransactionsForDateRange(startDate, endDate));
        }
    }
}