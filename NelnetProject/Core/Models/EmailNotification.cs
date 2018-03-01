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
    }
}
