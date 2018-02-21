using System;
using Accessors;
using Core.Models;
using System.Web.Http;
using Core.Interfaces;
using Engines;

namespace Web.Controllers
{
    /// <summary>
    /// IN PROGRESS CLASS USED FOR TESTING.
    /// NOT TO BE USED IN FINAL APP.
    /// </summary>
    [RoutePrefix("api/email")]
    public class EmailController : ApiController
    {

        private INotificationEngine notificationEngine;

        public EmailController(INotificationEngine notificationEngine)
        {
            this.notificationEngine = notificationEngine;
        }

        [HttpPost]
        [Route("send")]
        public void Post([FromBody] EmailNotification email)
        {
            ((NotificationEngine) notificationEngine).GetEmailAccessor().SendEmail(email); //todo fix this once testing is done
        }

    }
}