﻿using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Web.Controllers
{
    /// <summary>
    /// Utility class for controllers to get information from HTTP results.
    /// </summary>
    public class ControllerUtils
    {
        /// <summary>
        /// Gets an user ID from an http result.
        /// </summary>
        public static string httpGetUserID(ClaimsIdentity user)
        {
            string userID = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userID;
        }

        /// <summary>
        /// Gets an email from an http result
        /// </summary>
        public static string httpGetEmail(ClaimsIdentity user)
        {
            string email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return email;
        }
    }
}