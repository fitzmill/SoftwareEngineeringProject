using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class EmailNotification
    {
        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

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
