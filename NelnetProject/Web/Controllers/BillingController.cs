using Core.DTOs;
using Core.Interfaces;
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
    /// <summary>
    /// Controller for getting/setting billing information with PaymentSpring.
    /// </summary>
    public class BillingController : ApiController
    {
        IGetUserInfoEngine _getUserInfoEngine;
        ISetUserInfoEngine _setUserInfoEngine;
        INotificationEngine _notificationEngine;

        public BillingController(IGetUserInfoEngine getUserInfoEngine, ISetUserInfoEngine setUserInfoEngine, INotificationEngine notificationEngine)
        {
            _getUserInfoEngine = getUserInfoEngine;
            _setUserInfoEngine = setUserInfoEngine;
            _notificationEngine = notificationEngine;
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

            return Ok(_getUserInfoEngine.GetPaymentInfoForUser(parsedUserID));
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

            _setUserInfoEngine.UpdatePaymentBillingInfo(paymentAddressDTO);

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

            _setUserInfoEngine.UpdatePaymentCardInfo(paymentCardDTO);

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

            _setUserInfoEngine.InsertPaymentInfo(userPaymentInfo);

            return Ok();
        }

    }
}
