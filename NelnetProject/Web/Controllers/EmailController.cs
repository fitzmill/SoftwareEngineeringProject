using Core;
using Core.Interfaces;
using Core.Models;
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
            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(new Transaction());
            notificationEngine.SendTransactionNotifications(transactions);
            return Ok(email);
        }
    }
}
