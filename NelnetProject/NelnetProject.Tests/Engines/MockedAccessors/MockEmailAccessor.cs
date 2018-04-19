using Core.Interfaces.Accessors;
using Core.Models;
using System.Collections.Generic;

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
