using Core;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engines.Utils
{
    //Util class for generating email notifications
    public class EmailUtil
    {
        //Generates email notification for an upcoming payment
        public static EmailNotification UpcomingPaymentNotification(Transaction t, User user)
        {
            String subject = "Alert from Tuition Assistant: Upcoming Payment";
            String rawBody = String.Format("You have an upcoming payment.\nDate: {0:MMMM d yyyy}\nAmount: ${1}\nYou don't need to worry about anything. We'll charge your card automatically on this date.", t.DateDue, t.AmountCharged);
            return GenerateEmail(user.Email, subject, rawBody, user.FirstName);
        }

        //Generates email notification for a successfully charged payment
        public static EmailNotification PaymentChargedSuccessfullyNotification(Transaction t, User user)
        {
            String subject = "Alert from Tuition Assistnat: Payment Successful";
            String rawBody = String.Format("Congratulations! Your payment was processed succesfully.\nDate: {0:MMMM d yyyy}\nAmount: ${1}", t.DateCharged, t.AmountCharged);
            return GenerateEmail(user.Email, subject, rawBody, user.FirstName);
        }

        //Generates email notification for an unsuccessful payment that is being retried
        public static EmailNotification PaymentUnsuccessfulRetryingNotification(Transaction t, User user, DateTime today)
        {
            int daysRemaining = TuitionUtil.DUE_DAY - TuitionUtil.DaysOverdue(t, today);
            String subject = String.Format("Alert from Tuition Assistant: Payment Unsuccessful ({0} DAYS REMAINING)", daysRemaining);
            String rawBody = String.Format("There was an issue with your credit card. Your payment on {0:MMMM d yyyy} for ${1} failed for the following reason: {2}." +
                "\nPlease resolve the issue as soon as possible.\nIf the payment remains unsuccessful after {3} more days, the amound will be deferred and a late fee of ${4} will be added.",
                t.DateCharged, t.AmountCharged, t.ReasonFailed, daysRemaining, TuitionUtil.LATE_FEE);
            return GenerateEmail(user.Email, subject, rawBody, user.FirstName);
        }

        //Generates email notification for an unsuccessful payment that has been deferred
        public static EmailNotification PaymentFailedNotification(Transaction t, User user)
        {
            String subject = "Alert from Tuition Assistant: Payment Failed";
            String rawBody = String.Format("Your payment of ${0} that was due on {1:MMMM d yyyy} failed for {2} days and has been deferred." +
                "\nThe amount will be added to your next payment, along with a late fee of ${3}.", t.AmountCharged, t.DateDue, TuitionUtil.OVERDUE_RETRY_PERIOD, TuitionUtil.LATE_FEE);
            return GenerateEmail(user.Email, subject, rawBody, user.FirstName);
        }

        //Generates email notification with the default Tuition Assistant body template
        public static EmailNotification GenerateEmail(String to, String subject, String rawBody, String userFirstName)
        {
            String body = String.Format("Hi {0},\n{1}\nPlease contact us if you have any questions.\nPowered by Tuition Assistant\n", userFirstName, rawBody);
            return new EmailNotification()
            {
                To = to,
                Subject = subject,
                Body = body
            };
        }
    }
}
