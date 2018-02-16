using System;
using Accessors;
using Core.Models;
using System.Web.Http;

namespace Web.Controllers
{
    [RoutePrefix("api/email")]
    public class EmailController : ApiController
    {

        [HttpPost]
        [Route("send")]
        public void Post([FromBody] EmailNotification email)
        {
            EmailAccessor accessor = new EmailAccessor();
            Console.WriteLine(email);
            accessor.SendEmail(email);
            //accessor.SendEmail(new EmailNotification(to, subject, body));
        }

    }
}