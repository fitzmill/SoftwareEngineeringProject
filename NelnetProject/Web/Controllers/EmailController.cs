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
            throw new Exception("AHHHH");
        }

        [HttpGet]
        [Route("")]
        public string test()
        {
            return "test";
        }

        [HttpPost]
        [Route("send")]
        public void Post([FromBody] EmailNotification email)
        {
            ((NotificationEngine)notificationEngine).GetEmailAccessor().SendEmail(email); //todo fix this once testing is done
        }
    }
}
