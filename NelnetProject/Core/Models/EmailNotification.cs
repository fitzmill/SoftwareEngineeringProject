using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    /// <summary>
    /// Model for email notifications
    /// </summary>
    public class EmailNotification
    {
        /// <summary>
        /// The Email Recipient
        /// </summary>
        [Required]
        public string To { get; set; }

        /// <summary>
        /// The Email Subject
        /// </summary>
        [Required]
        public string Subject { get; set; }

        /// <summary>
        /// The Email Body
        /// </summary>
        [Required]
        public string Body { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var notification = obj as EmailNotification;
            return notification != null &&
                   To == notification.To &&
                   Subject == notification.Subject &&
                   Body == notification.Body;
        }

        public override int GetHashCode()
        {
            var hashCode = 1693610592;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(To);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Subject);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Body);
            return hashCode;
        }
    }
}
