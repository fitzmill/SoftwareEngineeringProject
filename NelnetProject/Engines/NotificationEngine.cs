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
        private readonly IEmailAccessor _emailAccessor;
        private readonly IUserAccessor _userAccessor;

        public NotificationEngine(IEmailAccessor emailAccessor, IUserAccessor getUserInfoAccessor)
        {
            _emailAccessor = emailAccessor;
            _userAccessor = getUserInfoAccessor;
        }

        public void SendTransactionNotifications(IList<Transaction> transactions)
        {
            foreach (Transaction t in transactions)
            {
                ProcessState state = t.ProcessState;
                User user = _userAccessor.GetUserInfoByID(t.UserID);

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

                _emailAccessor.SendEmail(email);
            }
        }

        public void SendAccountUpdateNotification(string email, string firstName, string informationType)
        {
            EngineArgumentValidation.StringIsNotEmpty(email, "email");
            EngineArgumentValidation.StringIsNotEmpty(firstName, "first name");
            EngineArgumentValidation.StringIsNotEmpty(informationType, "information type");
            var emailNotification = EmailUtil.AccountUpdatedNotification(email, firstName, informationType);
            _emailAccessor.SendEmail(emailNotification);
        }

        public void SendAccountCreationNotification(User user, DateTime today)
        {
            EngineArgumentValidation.ArgumentIsNotNull(user, "user");
            Transaction nextTransaction = new Transaction()
            {
                AmountCharged = TuitionUtil.GenerateAmountDue(user, TuitionUtil.DEFAULT_PRECISION),
                DateDue = TuitionUtil.NextPaymentDueDate(user.Plan, today)
            };
            EmailNotification email = EmailUtil.AccountCreatedNotification(user, nextTransaction);
            _emailAccessor.SendEmail(email);
        }

        public void SendAccountDeletionNotification(User user)
        {
            EngineArgumentValidation.ArgumentIsNotNull(user, "user");
            EmailNotification email = EmailUtil.AccountDeletedNotification(user);
            _emailAccessor.SendEmail(email);
        }

    }
}
