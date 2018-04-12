using Core.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
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
        public IHttpActionResult ValidateLoginInfo([Required] LoginDTO loginDTO)
        {
            if (loginDTO == null || !ModelState.IsValid)
            {
                return BadRequest("One or more required objects was not included in the request body.");
            }
            string email = loginDTO.Email;
            string password = loginDTO.Password;
            bool result;
            try
            {
                result = getUserInfoEngine.ValidateLoginInfo(email, password);
            }
            catch (SqlException)
            {
                return InternalServerError();
            }
            return Ok(result);
        }
    }
}
