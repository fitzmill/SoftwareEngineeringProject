using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        IGetUserInfoEngine getUserInfoEngine;
        public LoginController(IGetUserInfoEngine getUserInfoEngine)
        {
            this.getUserInfoEngine = getUserInfoEngine;
        }
        [HttpGet]
        [Route("ValidateLoginInfo/{email, password}")]
        public IHttpActionResult ValidateLoginInfo(string email, string password)
        {
            return Ok(getUserInfoEngine.ValidateLoginInfo(email, password));
        }
    }
}
