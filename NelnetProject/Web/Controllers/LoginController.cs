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

        [HttpPost]
        [Route("ValidateLoginInfo")]
        [AllowAnonymous]
        public IHttpActionResult ValidateLoginInfo([Required] LoginDTO loginDTO)
        {
            AuthenticationHeaderValue authenticationHeader = Request.Headers.Authorization;

            if (authenticationHeader == null || !authenticationHeader.Scheme.StartsWith("Basic"))
            {
                return Unauthorized();
            }

            var encodedUsernamePassword = authenticationHeader.Parameter;
            var encoding = Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

            var seperatorIndex = usernamePassword.IndexOf(':');

            string username = usernamePassword.Substring(0, seperatorIndex);
            string password = usernamePassword.Substring(seperatorIndex + 1);

            if (getUserInfoEngine.ValidateLoginInfo(loginDTO.Email, loginDTO.Password))
            {
                return Ok(JwtManager.GenerateToken(loginDTO.Email));
            }

            return Unauthorized();
        }
    }
}
