using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Interfaces;
using Core.Models;
using Engines.Utils;

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
            transactions.ForEach (t => {
                ProcessState state = t.ProcessState;
                User user = getUserInfoAccessor.GetUserInfoByID(t.UserID);

                EmailNotification email;
                if (state == ProcessState.NOT_YET_CHARGED)
                {
                    email = EmailUtil.UpcomingPaymentNotification(t, user);
                }
                else if (state == ProcessState.SUCCESSFUL)
                {
                    email = EmailUtil.PaymentChargedSuccessfullyNotification(t, user);
                }
                else if (state == ProcessState.RETRYING)
                {
                    email = EmailUtil.PaymentUnsuccessfulRetryingNotification(t, user, t.DateCharged.GetValueOrDefault(DateTime.Today));
                }
                else
                {
                    email = EmailUtil.PaymentFailedNotification(t, user);
                }

                emailAccessor.SendEmail(email);
            });
        }

        public void SendAccountUpdateNotification(ClaimsIdentity user, string informationType)
        {
            var email = EmailUtil.AccountUpdatedNotification(user, informationType);
            emailAccessor.SendEmail(email);
        }
    }
}
