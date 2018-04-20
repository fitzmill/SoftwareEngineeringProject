using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Web.Controllers
{
    public class ControllerUtils
    {
        public static string httpGetUserID(ClaimsIdentity user)
        {
            string userID = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userID;
        }

        public static string httpGetEmail(ClaimsIdentity user)
        {
            string email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return email;
        }
    }
}