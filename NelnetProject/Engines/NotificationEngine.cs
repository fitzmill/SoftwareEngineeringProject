using System;
using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Core.Interfaces.Accessors;
using Core.Interfaces.Engines;
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

        public void SendTransactionNotifications(IList<Transaction> transactions)
        {
            foreach (Transaction t in transactions)
            {
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
            }
        }

        public void SendAccountUpdateNotification(string email, string firstName, string informationType)
        {
            var emailNotification = EmailUtil.AccountUpdatedNotification(email, firstName, informationType);
            emailAccessor.SendEmail(emailNotification);
        }

        public void SendAccountCreationNotification(User user, DateTime today)
        {
            Transaction nextTransaction = new Transaction()
            {
                AmountCharged = TuitionUtil.GenerateAmountDue(user, TuitionUtil.DEFAULT_PRECISION),
                DateDue = TuitionUtil.NextPaymentDueDate(user.Plan, today)
            };
            EmailNotification email = EmailUtil.AccountCreatedNotification(user, nextTransaction);
            emailAccessor.SendEmail(email);
        }

        public void SendAccountDeletionNotification(User user)
        {
            EmailNotification email = EmailUtil.AccountDeletedNotification(user);
            emailAccessor.SendEmail(email);
        }

    }
}
