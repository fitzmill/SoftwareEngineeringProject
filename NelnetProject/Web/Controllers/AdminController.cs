using Accessors;
using Core;
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
            int intUserID = 0;
            //Tries to convert the parameter to an int
            if (!int.TryParse(userID, out intUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }
            return Ok(getTransactionEngine.GetAllTransactionsForUser(intUserID));
        }
    }
}