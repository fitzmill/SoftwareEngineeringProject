using Core;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces;
using Newtonsoft.Json.Linq;
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
        ISetUserInfoEngine setUserInfoEngine;
        IPaymentEngine paymentEngine;

        public AccountController(IGetUserInfoEngine getUserInfoEngine, ISetUserInfoEngine setUserInfoEngine, IGetTransactionEngine getTransactionEngine, IPaymentEngine paymentEngine)
        {
            this.getUserInfoEngine = getUserInfoEngine;
            this.getTransactionEngine = getTransactionEngine;
            this.paymentEngine = paymentEngine;
            this.setUserInfoEngine = setUserInfoEngine;
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
        [Route("UpdatePersonalInfo")]
        public IHttpActionResult UpdatePersonalInfo(User user)
        {
            if (!IsValidUserObject(user))
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }

            try
            {
                setUserInfoEngine.UpdatePersonalInfo(user);
            }
            catch (SqlRowNotAffectedException srnae)
            {
                return BadRequest(srnae.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("UpdateStudentInfo")]
        public IHttpActionResult UpdateStudentInfo(UpdateStudentInfoDTO updatedInfo)
        {
            if (updatedInfo == null || updatedInfo.UpdatedStudents == null || updatedInfo.DeletedStudentIDs == null || updatedInfo.AddedStudents == null ||
                updatedInfo.UpdatedStudents.Any(s => !IsValidStudentObject(s)) || updatedInfo.AddedStudents.Any(s => !IsValidStudentObject(s)))
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }

            try
            {
                setUserInfoEngine.UpdateStudentInfo(updatedInfo.UpdatedStudents);
                setUserInfoEngine.InsertStudentInfo(updatedInfo.UserID, updatedInfo.AddedStudents);
                setUserInfoEngine.DeleteStudentInfo(updatedInfo.DeletedStudentIDs);
            }
            catch (SqlRowNotAffectedException srnae)
            {
                return BadRequest(srnae.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("DeleteUser")]
        public IHttpActionResult DeleteUser(User user)
        {
            if (!IsValidUserObject(user))
            {
                return BadRequest("One or more required objects was not included in the request body");
            }

            try
            {
                setUserInfoEngine.DeletePersonalInfo(user.UserID, user.CustomerID);
            }
            catch (SqlRowNotAffectedException srnae)
            {
                return BadRequest(srnae.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("InsertPersonalInfo")]
        public IHttpActionResult InsertPersonalInfo(User user)
        {
            if (!IsValidUserObject(user))
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            
            setUserInfoEngine.InsertPersonalInfo(user);
            return Ok();
        }

        [HttpPost]
        [Route("GetUserInfoByEmail")]
        public IHttpActionResult GetUserInfoByEmail([FromBody] string email)
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

        [HttpPost]
        [Route("UpdatePaymentBillingInfo")]
        public IHttpActionResult UpdatePaymentInfo(PaymentAddressDTO paymentAddressDTO)
        {
            setUserInfoEngine.UpdatePaymentBillingInfo(paymentAddressDTO);
            return Ok();
        }

        [HttpPost]
        [Route("UpdatePaymentCardInfo")]
        public IHttpActionResult UpdatePaymentCardInfo(PaymentCardDTO paymentCardDTO)
        {
            setUserInfoEngine.UpdatePaymentCardInfo(paymentCardDTO);
            return Ok();
        }

        [HttpPost]
        [Route("InsertPaymentInfo")]
        public IHttpActionResult InsertPaymentInfo(UserPaymentInfoDTO userPaymentInfo)
        {
            if (!IsValidPaymentInfoObject(userPaymentInfo))
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            setUserInfoEngine.InsertPaymentInfo(userPaymentInfo);
            return Ok();
        }

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
        [HttpGet]
        [Route("GetNextTransactionForUser/{userID}")]
        public IHttpActionResult GetNextTransactionForUser(string userID)
        {
            if (!int.TryParse(userID, out int parsedUserID))
            {
                return BadRequest("Could not parse userID into an integer");
            }
            return Ok(paymentEngine.CalculateNextPaymentForUser(parsedUserID, DateTime.Now));
        }

        //POST api/account/CalculatePeriodicPayment
        [HttpPost]
        [Route("CalculatePeriodicPayment")]
        public IHttpActionResult CalculatePeriodicPayment([FromBody] User user)
        {
            return Ok(paymentEngine.CalculatePeriodicPayment(user));
        }

        private bool IsValidUserObject(User user)
        {
            bool validUser = user != null && !String.IsNullOrEmpty(user.FirstName) && !String.IsNullOrEmpty(user.LastName) &&
                !String.IsNullOrEmpty(user.CustomerID) && !String.IsNullOrEmpty(user.Email) && !String.IsNullOrEmpty(user.Hashed) &&
                !String.IsNullOrEmpty(user.Salt);

            //all students are valid
            bool validStudents = validUser ?  user.Students != null && user.Students.All(s => IsValidStudentObject(s)) : false;

            return validUser && validStudents;
        }

        private bool IsValidStudentObject(Student student)
        {
            return !String.IsNullOrEmpty(student.FirstName) && !String.IsNullOrEmpty(student.LastName);
        }

        private bool IsValidPaymentInfoObject(UserPaymentInfoDTO paymentInfo)
        {
            return paymentInfo != null && !String.IsNullOrEmpty(paymentInfo.FirstName) && !String.IsNullOrEmpty(paymentInfo.LastName) &&
                !String.IsNullOrEmpty(paymentInfo.State) && !String.IsNullOrEmpty(paymentInfo.StreetAddress1) &&
                !String.IsNullOrEmpty(paymentInfo.StreetAddress2) && !String.IsNullOrEmpty(paymentInfo.Zip);
        }
    }
}