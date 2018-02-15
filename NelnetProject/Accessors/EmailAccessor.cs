using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;

namespace Accessors
{
    /// <summary>
    /// Accessor to email sending service.
    /// </summary>
    public class EmailAccessor : IEmailAccessor
    {
        
        public void SendEmail(EmailNotification emailNotification)
        {
            //Sends email notification to external email API
            throw new NotImplementedException();
        }
    }
}
