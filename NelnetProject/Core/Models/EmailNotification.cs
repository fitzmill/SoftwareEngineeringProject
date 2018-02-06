using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class EmailNotification
    {
        string To { get; set; }

        string Subject { get; set; }

        string Body { get; set; }
    }
}
