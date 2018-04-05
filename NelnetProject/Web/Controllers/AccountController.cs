using Core;
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
        IPaymentEngine paymentEngine;

        public AccountController(IGetUserInfoEngine getUserInfoEngine, IGetTransactionEngine getTransactionEngine, IPaymentEngine paymentEngine)
        {
            this.getUserInfoEngine = getUserInfoEngine;
            this.getTransactionEngine = getTransactionEngine;
            this.paymentEngine = paymentEngine;
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

        [HttpPost]
        [Route("GetUserInfoByEmail")]
        public IHttpActionResult GetUserInfoByEmail([FromBody]string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return BadRequest("Email was not supplied");
            }
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

        // GET api/account/GetAllTransactionsForUser/5
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

        //GET api/account/GetNextTransactionForUser/{userID}
        [Route("GetNextTransactionForUser/{userID}")]
        public IHttpActionResult GetNextTransactionForUser(string userID)
        {
            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }
            return Ok(paymentEngine.CalculateNextPaymentForUser(parsedUserID, DateTime.Now));
        }
    }
}