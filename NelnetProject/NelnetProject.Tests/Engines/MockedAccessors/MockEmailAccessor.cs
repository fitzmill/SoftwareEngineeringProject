using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    class MockEmailAccessor : IEmailAccessor
    {
        public List<EmailNotification> emails = new List<EmailNotification>();


        public void SendEmail(EmailNotification emailNotification)
        {
            emails.Add(emailNotification);
        }
    }
}
