using Core;
using Core.DTOs;
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
        ISetUserInfoEngine setUserInfoEngine;

        public AccountController(IGetUserInfoEngine getUserInfoEngine, ISetUserInfoEngine setUserInfoEngine)
        {
            this.getUserInfoEngine = getUserInfoEngine;
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
        [Route("UpdatePersonalAndStudentInfo")]
        public IHttpActionResult UpdatePersonalAndStudentInfo(User user)
        {
            if (user == null || user.FirstName == null || user.LastName == null || user.CustomerID == null || user.Email == null || user.Hashed == null || user.Salt == null)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            foreach(Student s in user.Students)
            {
                if (s.FirstName == null || s.LastName == null)
                {
                    return BadRequest("One or more required objects was not included in the request body.");
                }
            }
            setUserInfoEngine.UpdatePersonalInfo(user);
            return Ok();
        }
        [HttpPost]
        [Route("InsertPersonalInfo")]
        public IHttpActionResult InsertPersonalInfo(User user)
        {
            if (user == null || user.FirstName == null || user.LastName == null || user.CustomerID == null || user.Email == null || user.Hashed == null || user.Salt == null)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            foreach (Student s in user.Students)
            {
                if (s.FirstName == null || s.LastName == null)
                {
                    return BadRequest("One or more required objects was not included in the request body.");
                }
            }
            setUserInfoEngine.InsertPersonalInfo(user);
            return Ok();
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
        [HttpPost]
        [Route("UpdatePaymentInfo")]
        public IHttpActionResult UpdatePaymentInfo(UserPaymentInfoDTO userPaymentInfo)
        {
            if (userPaymentInfo.FirstName == null || userPaymentInfo.LastName == null || userPaymentInfo.State == null || userPaymentInfo.StreetAddress1 == null || userPaymentInfo.StreetAddress2 == null || userPaymentInfo.Zip == null)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            setUserInfoEngine.UpdatePaymentInfo(userPaymentInfo);
            return Ok();
        }
        [HttpPost]
        [Route("InsertPaymentInfo")]
        public IHttpActionResult InsertPaymentInfo(UserPaymentInfoDTO userPaymentInfo)
        {
            if (userPaymentInfo.FirstName == null || userPaymentInfo.LastName == null || userPaymentInfo.State == null || userPaymentInfo.StreetAddress1 == null || userPaymentInfo.StreetAddress2 == null || userPaymentInfo.Zip == null)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            setUserInfoEngine.InsertPaymentInfo(userPaymentInfo);
            return Ok();
        }
    }
}