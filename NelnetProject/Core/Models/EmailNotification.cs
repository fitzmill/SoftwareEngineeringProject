using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    /// <summary>
    /// Model for email notifications to be sent to users.
    /// </summary>
    public class EmailNotification
    {
        /// <summary>
        /// The user's email address
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Subject line of the email
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body of the email, represented as a single string
        /// </summary>
        public string Body { get; set; }
    }
}
