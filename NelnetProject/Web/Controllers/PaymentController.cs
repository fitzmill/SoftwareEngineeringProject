using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Web.Filters;

namespace Web.Controllers
{
    [RoutePrefix("api/payment")]
    [SqlRowNotAffectedFilter]
    public class PaymentController : ApiController
    {

        //This is a get request with the above route. The 5 at the end of the example is an example userID
        [HttpGet]
        [Route("GetAllTransactionsForUser")]
        [JwtAuthentication]
        public IHttpActionResult GetAllTransactionsForUser()
        {
            var user = (ClaimsIdentity)User.Identity;
            var userID = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }
            return Ok(getTransactionEngine.GetAllTransactionsForUser(parsedUserID));
        }

        //GET api/account/GetNextTransactionForUser/{userID}
        [HttpGet]
        [Route("GetNextTransactionForUser")]
        [JwtAuthentication]
        public IHttpActionResult GetNextTransactionForUser()
        {
            var user = (ClaimsIdentity)User.Identity;
            var userID = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }
            return Ok(paymentEngine.CalculateNextPaymentForUser(parsedUserID, DateTime.Now));
        }

        //POST api/account/CalculatePeriodicPayment
        [HttpPost]
        [Route("CalculatePeriodicPayment")]
        [AllowAnonymous]
        public IHttpActionResult CalculatePeriodicPayment(User user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            return Ok(paymentEngine.CalculatePeriodicPayment(user));
        }

    }
}
