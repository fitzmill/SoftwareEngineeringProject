using Core;
using Core.Interfaces;
using Core.Interfaces.Engines;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Web.Filters;

namespace Web.Controllers
{
    /// <summary>
    /// Controls methods for retrieving and calculating payments (a.k.a. transactions).
    /// </summary>
    [RoutePrefix("api/payment")]
    [SqlRowNotAffectedFilter]
    public class PaymentController : ApiController
    {
        IGetTransactionEngine _getTransactionEngine;
        IPaymentEngine _paymentEngine;

        public PaymentController(IGetTransactionEngine getTransactionEngine, IPaymentEngine paymentEngine)
        {
            _getTransactionEngine = getTransactionEngine;
            _paymentEngine = paymentEngine;
        }

        /// <summary>
        /// Gets all transactions for the given user.
        /// </summary>
        /// <returns>A list of all of the user's transactions</returns>
        [HttpGet]
        [Route("GetAllTransactionsForUser")]
        [JwtAuthentication]
        public IHttpActionResult GetAllTransactionsForUser()
        {
            var user = (ClaimsIdentity) User.Identity;
            var userID = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }

            return Ok(_getTransactionEngine.GetAllTransactionsForUser(parsedUserID));
        }

        /// <summary>
        /// Gets the next payment for the given user.
        /// </summary>
        /// <returns>The next payment due for the user</returns>
        [HttpGet]
        [Route("GetNextPaymentForUser")]
        [JwtAuthentication]
        public IHttpActionResult GetNextPaymentForUser()
        {
            var user = (ClaimsIdentity)User.Identity;
            var userID = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }

            return Ok(_paymentEngine.CalculateNextPaymentForUser(parsedUserID, DateTime.Now));
        }

        /// <summary>
        /// Calculate what the periodic payment will be for the given user.
        /// </summary>
        /// <param name="user">the user</param>
        /// <returns>the amount due each period</returns>
        [HttpPost]
        [Route("CalculatePeriodicPayment")]
        [AllowAnonymous]
        public IHttpActionResult CalculatePeriodicPayment(User user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }

            return Ok(_paymentEngine.CalculatePeriodicPayment(user));
        }

    }
}
