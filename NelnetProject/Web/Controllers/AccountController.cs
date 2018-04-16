﻿using Core;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces;
using Newtonsoft.Json.Linq;
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
    [RoutePrefix("api/account")]
    [SqlRowNotAffectedFilter]
    public class AccountController : ApiController
    {
        IGetUserInfoEngine getUserInfoEngine;
        IGetTransactionEngine getTransactionEngine;
        ISetUserInfoEngine setUserInfoEngine;
        IPaymentEngine paymentEngine;
        INotificationEngine notificationEngine;

        public AccountController(IGetUserInfoEngine getUserInfoEngine, 
            ISetUserInfoEngine setUserInfoEngine, 
            IGetTransactionEngine getTransactionEngine, 
            IPaymentEngine paymentEngine,
            INotificationEngine notificationEngine)
        {
            this.getUserInfoEngine = getUserInfoEngine;
            this.getTransactionEngine = getTransactionEngine;
            this.paymentEngine = paymentEngine;
            this.setUserInfoEngine = setUserInfoEngine;
            this.notificationEngine = notificationEngine;
        }

        [HttpGet]
        [Route("GetUserInfo")]
        [JwtAuthentication]
        public IHttpActionResult GetUserInfo()
        {
            var user = (ClaimsIdentity)User.Identity;
            var userID = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userID == null || !int.TryParse(userID, out int parsedUserID))
            {
                return Unauthorized();
            }
            return Ok(getUserInfoEngine.GetUserInfoByID(parsedUserID));
        }

        [HttpPost]
        [Route("EmailExists")]
        [AllowAnonymous]
        public IHttpActionResult EmailExists([FromBody] string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return BadRequest("Email was not supplied");
            }
            return Ok(getUserInfoEngine.EmailExists(email));
        }

        [HttpPost]
        [Route("UpdatePersonalInfo")]
        [JwtAuthentication]
        public IHttpActionResult UpdatePersonalInfo(User user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }

            setUserInfoEngine.UpdatePersonalInfo(user);

            var newToken = JwtManager.GenerateToken(user);

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            notificationEngine.SendAccountUpdateNotification(claimsIdentity, "personal");
            return Ok(newToken);
        }

        [HttpPost]
        [Route("UpdateStudentInfo")]
        [JwtAuthentication]
        public IHttpActionResult UpdateStudentInfo(UpdateStudentInfoDTO updatedInfo)
        {
            if (updatedInfo == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }

            setUserInfoEngine.UpdateStudentInfo(updatedInfo.UpdatedStudents);
            setUserInfoEngine.InsertStudentInfo(updatedInfo.UserID, updatedInfo.AddedStudents);
            setUserInfoEngine.DeleteStudentInfo(updatedInfo.DeletedStudentIDs);

            var user = (ClaimsIdentity)User.Identity;
            notificationEngine.SendAccountUpdateNotification(user, "student");
            return Ok();
        }

        [HttpPost]
        [Route("DeleteUser")]
        [JwtAuthentication]
        public IHttpActionResult DeleteUser(User user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body");
            }

            setUserInfoEngine.DeletePersonalInfo(user.UserID, user.CustomerID);
            return Ok();
        }

        [HttpPost]
        [Route("InsertUser")]
        [AllowAnonymous]
        public IHttpActionResult InsertUser([FromBody] AccountCreationDTO accountCreationInfo)
        {
            if (accountCreationInfo == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }

            UserPaymentInfoDTO paymentInfo = new UserPaymentInfoDTO
            {
                CustomerID = "", //This isn't neccessary for the creation of a customer in payment spring
                FirstName = accountCreationInfo.CardholderFirstName,
                LastName = accountCreationInfo.CardholderLastName,
                StreetAddress1 = accountCreationInfo.StreetAddress1,
                StreetAddress2 = accountCreationInfo.StreetAddress2,
                City = accountCreationInfo.City,
                State = accountCreationInfo.State,
                Zip = accountCreationInfo.Zip,
                CardNumber = accountCreationInfo.CardNumber,
                ExpirationYear = accountCreationInfo.ExpirationYear,
                ExpirationMonth = accountCreationInfo.ExpirationMonth,
                CardType = "" //This also isn't neccessary for the creation of a customer in payment spring
            };

            string customerID = setUserInfoEngine.InsertPaymentInfo(paymentInfo);

            //check if payment info is valid, if not return error
            if(customerID == null)
            {
                return BadRequest("Payment information is invalid.");
            }

            User user = new User
            {
                UserID = 0,
                FirstName = accountCreationInfo.FirstName,
                LastName = accountCreationInfo.LastName,
                Email = accountCreationInfo.Email,
                Hashed = "", //This should be set when the account is created
                Salt = "", //This should also be set when the account is created
                Plan = accountCreationInfo.Plan,
                UserType = accountCreationInfo.UserType,
                CustomerID = customerID,
                Students = accountCreationInfo.Students
            };
            
            setUserInfoEngine.InsertPersonalInfo(user, accountCreationInfo.Password);
            setUserInfoEngine.InsertStudentInfo(user.UserID, user.Students);

            var token = JwtManager.GenerateToken(user);

            return Ok(token);
        }

        [HttpGet]
        [Route("GetPaymentInfoForUser")]
        [JwtAuthentication]
        public IHttpActionResult GetPaymentInfoForUser()
        {
            var user = (ClaimsIdentity)User.Identity;
            var userID = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }
            return Ok(getUserInfoEngine.GetPaymentInfoForUser(parsedUserID));
        }

        [HttpPost]
        [Route("UpdatePaymentBillingInfo")]
        [JwtAuthentication]
        public IHttpActionResult UpdatePaymentBillingInfo(PaymentAddressDTO paymentAddressDTO)
        {
            if (paymentAddressDTO == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            setUserInfoEngine.UpdatePaymentBillingInfo(paymentAddressDTO);

            var user = (ClaimsIdentity)User.Identity;
            notificationEngine.SendAccountUpdateNotification(user, "billing");
            return Ok();
        }

        [HttpPost]
        [Route("UpdatePaymentCardInfo")]
        [JwtAuthentication]
        public IHttpActionResult UpdatePaymentCardInfo(PaymentCardDTO paymentCardDTO)
        {
            if (paymentCardDTO == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            setUserInfoEngine.UpdatePaymentCardInfo(paymentCardDTO);

            var user = (ClaimsIdentity)User.Identity;
            notificationEngine.SendAccountUpdateNotification(user, "payment");
            return Ok();
        }

        [HttpPost]
        [Route("InsertPaymentInfo")]
        [AllowAnonymous]
        public IHttpActionResult InsertPaymentInfo(UserPaymentInfoDTO userPaymentInfo)
        {
            if (userPaymentInfo == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            setUserInfoEngine.InsertPaymentInfo(userPaymentInfo);
            return Ok();
        }

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