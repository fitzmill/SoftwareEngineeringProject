using Core;
using Core.Interfaces;
using Core.Models;
using Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Controllers
{
    [RoutePrefix("api/email")]
    public class EmailController : ApiController
    {

        private INotificationEngine notificationEngine;

        public EmailController(INotificationEngine notificationEngine)
        {
            this.notificationEngine = notificationEngine;
        }

        [HttpGet]
        public string test()
        {
            return "test";
        }

        [HttpPost]
        public IHttpActionResult Post(EmailNotification email)
        {
            ((NotificationEngine)notificationEngine).GetEmailAccessor().SendEmail(email); //this is a hack to be used just for testing. It is not proper design.
            return Ok(email);
        }
    }
}
