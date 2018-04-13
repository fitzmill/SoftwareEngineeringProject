using Core;
using Core.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        [Route("ValidateLoginInfo")]
        [AllowAnonymous]
        public IHttpActionResult ValidateLoginInfo()
        {
            AuthenticationHeaderValue authenticationHeader = Request.Headers.Authorization;

            if (authenticationHeader == null || !authenticationHeader.Scheme.StartsWith("Basic"))
            {
                return Unauthorized();
            }

            var encodedUsernamePassword = authenticationHeader.Parameter;
            var encoding = Encoding.GetEncoding("iso-8859-1");
            string emailPassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

            var seperatorIndex = emailPassword.IndexOf(':');

            string email = emailPassword.Substring(0, seperatorIndex);
            string password = emailPassword.Substring(seperatorIndex + 1);

            var user = getUserInfoEngine.ValidateLoginInfo(email, password);
            if (user != null)
            {
                return Ok(new LoginDTO()
                {
                    JwtToken = JwtManager.GenerateToken(user),
                    UserType = user.UserType
                });

            }

            return Unauthorized();
        }
    }
}
