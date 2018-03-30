using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Interfaces;

namespace Engines
{
    /// <summary>
    /// Generates and sends notifications.
    /// </summary>
    public class NotificationEngine : INotificationEngine
    {
        private IEmailAccessor emailAccessor;
        private IGetUserInfoAccessor getUserInfoAccessor;

        public NotificationEngine(IEmailAccessor emailAccessor, IGetUserInfoAccessor getUserInfoAccessor)
        {
            this.emailAccessor = emailAccessor;
            this.getUserInfoAccessor = getUserInfoAccessor;
        }

        public void SendTransactionNotifications(List<Transaction> transactions)
        {
            //Convert transactions into notifications
            //Send notifications to appropriate accessor (email for now)
            throw new NotImplementedException();
        }

        public IEmailAccessor GetEmailAccessor()
        {
            return emailAccessor;
        }
    }
}
