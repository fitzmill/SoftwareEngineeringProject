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
        public AccountController(IGetUserInfoEngine getUserInfoEngine)
        {
            this.getUserInfoEngine = getUserInfoEngine;
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
    }
}