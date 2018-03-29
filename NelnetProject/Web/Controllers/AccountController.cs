using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        IGetUserInfoEngine getUserInfoEngine;
        IGetTransactionEngine getTransactionEngine;
        public AccountController(IGetUserInfoEngine getUserInfoEngine, IGetTransactionEngine getTransactionEngine)
        {
            this.getUserInfoEngine = getUserInfoEngine;
            this.getTransactionEngine = getTransactionEngine;
        }
        [HttpGet]
        [Route("GetUserInfoByID/{userID}")]
        public IHttpActionResult GetUserInfoByID(string userID)
        {
            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }
            return Ok(getUserInfoEngine.GetUserInfoByID(parsedUserID));
        }
        [HttpGet]
        [Route("GetUserInfoByEmail/{email}")]
        public IHttpActionResult GetUserInfoByEmail(string email)
        {
            return Ok(getUserInfoEngine.GetUserInfoByEmail(email));
        }
        [HttpGet]
        [Route("GetPaymentInfoForUser/{userID}")]
        public IHttpActionResult GetPaymentInfoForUser(string userID)
        {
            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }
            return Ok(getUserInfoEngine.GetPaymentInfoForUser(parsedUserID));
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
    }
}