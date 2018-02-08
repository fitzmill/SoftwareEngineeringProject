using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface for the accessor to the email sending service.
    /// </summary>
    interface IEmailAccessor
    {
        /// <summary>
        /// Sends an email notification.
        /// </summary>
        /// <param name="emailNotification"></param>
        void SendEmail(EmailNotification emailNotification);
    }
}
