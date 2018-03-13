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
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        IGetUserInfoEngine getUserInfoEngine;
        public LoginController(IGetUserInfoEngine getUserInfoEngine)
        {
            this.getUserInfoEngine = getUserInfoEngine;
        }

        [HttpPost]
        [Route("ValidateLoginInfo")]
        public IHttpActionResult ValidateLoginInfo(LoginDTO loginDTO)
        {
            if (loginDTO == null || loginDTO.Email == null || loginDTO.Password == null)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            string email = loginDTO.Email;
            string password = loginDTO.Password;
            return Ok(getUserInfoEngine.ValidateLoginInfo(email, password));
        }
    }
}
