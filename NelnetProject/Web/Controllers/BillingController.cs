using Core.DTOs;
using Core.Interfaces;
using Core.Interfaces.Accessors;
using Core.Interfaces.Engines;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Web.Filters;

namespace Web.Controllers
{
    /// <summary>
    /// Controller for getting/setting billing information with PaymentSpring.
    /// </summary>
    [RoutePrefix("api/billing")]
    [SqlRowNotAffectedFilter]
    public class BillingController : ApiController
    {
        private readonly INotificationEngine _notificationEngine;
        private readonly IPaymentEngine _paymentEngine;

        public BillingController(INotificationEngine notificationEngine,
            IPaymentEngine paymentEngine)
        {
            _notificationEngine = notificationEngine;
            _paymentEngine = paymentEngine;
        }

        /// <summary>
        /// Gets a user's payment info from PaymentSpring.
        /// </summary>
        /// <returns>the payment info</returns>
        [HttpGet]
        [Route("GetPaymentInfoForUser")]
        [JwtAuthentication]
        public IHttpActionResult GetPaymentInfoForUser()
        {
            var user = (ClaimsIdentity) User.Identity;
            var userID = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }

            return Ok(_paymentEngine.GetPaymentInfoForUser(parsedUserID));
        }

        /// <summary>
        /// Updates a user's billing information in PaymentSpring.
        /// </summary>
        /// <param name="paymentAddressDTO">the billing information</param>
        /// <returns>a success response</returns>
        [HttpPost]
        [Route("UpdatePaymentBillingInfo")]
        [JwtAuthentication]
        public IHttpActionResult UpdatePaymentBillingInfo(PaymentAddressDTO paymentAddressDTO)
        {
            if (paymentAddressDTO == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }

            _paymentEngine.UpdatePaymentBillingInfo(paymentAddressDTO);

            var user = (ClaimsIdentity)User.Identity;
            _notificationEngine.SendAccountUpdateNotification
                (user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value, user.Name, "billing");

            return Ok();
        }

        /// <summary>
        /// Updates a user's payment card information in PaymentSpring.
        /// </summary>
        /// <param name="paymentCardDTO">the payment card information</param>
        /// <returns>a success response</returns>
        [HttpPost]
        [Route("UpdatePaymentCardInfo")]
        [JwtAuthentication]
        public IHttpActionResult UpdatePaymentCardInfo(PaymentCardDTO paymentCardDTO)
        {
            if (paymentCardDTO == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }

            _paymentEngine.UpdatePaymentCardInfo(paymentCardDTO);

            var user = (ClaimsIdentity)User.Identity;
            _notificationEngine.SendAccountUpdateNotification
                (user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value, user.Name, "payment");

            return Ok();
        }

        /// <summary>
        /// Inserts a user's payment information into PaymentSpring.
        /// </summary>
        /// <param name="userPaymentInfo">the payment information</param>
        /// <returns>a success response</returns>
        [HttpPost]
        [Route("InsertPaymentInfo")]
        [AllowAnonymous]
        public IHttpActionResult InsertPaymentInfo(UserPaymentInfoDTO userPaymentInfo)
        {
            if (userPaymentInfo == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }

            _paymentEngine.InsertPaymentInfo(userPaymentInfo);

            return Ok();
        }

    }
}
