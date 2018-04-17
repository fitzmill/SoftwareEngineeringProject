using Core;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            if (t == null)
            {
                throw new ArgumentNullException("Transaction cannot be null.");
            }
            if (user == null)
            {
                throw new ArgumentNullException("User cannot be null.");
            }

            string subject = "Alert from Tuition Assistant: Upcoming Payment";
            string rawBody = string.Format("You have an upcoming payment.<br><br>Date: {0:MMMM d yyyy}<br>Amount: ${1:f2}<br><br>You don't need to worry about anything. We'll charge your card automatically on this date.", t.DateDue, t.AmountCharged);
            return GenerateEmail(user.Email, subject, rawBody, user.FirstName);
        }

        //Generates email notification for a successfully charged payment
        public static EmailNotification PaymentChargedSuccessfullyNotification(Transaction t, User user)
        {
            if (t == null)
            {
                throw new ArgumentNullException("Transaction cannot be null.");
            }
            if (user == null)
            {
                throw new ArgumentNullException("User cannot be null.");
            }

            string subject = "Alert from Tuition Assistant: Payment Successful";
            string rawBody = string.Format("Congratulations! Your payment was processed succesfully.<br><br>Date: {0:MMMM d yyyy}<br><br>Amount: ${1:f2}", t.DateCharged, t.AmountCharged);
            return GenerateEmail(user.Email, subject, rawBody, user.FirstName);
        }

        //Generates email notification for an unsuccessful payment that is being retried
        public static EmailNotification PaymentUnsuccessfulRetryingNotification(Transaction t, User user, DateTime today)
        {
            if (t == null)
            {
                throw new ArgumentNullException("Transaction cannot be null.");
            }
            if (user == null)
            {
                throw new ArgumentNullException("User cannot be null.");
            }

            int daysRemaining = TuitionUtil.OVERDUE_RETRY_PERIOD - TuitionUtil.DaysOverdue(t, today);
            string subject = string.Format("Alert from Tuition Assistant: Payment Unsuccessful ({0} DAY{1} REMAINING)", daysRemaining, daysRemaining == 1 ? "" : "S");
            string rawBody = string.Format("There was an issue with your credit card. Your payment on {0:MMMM d yyyy} for ${1:f2} failed for the following reason: {2}" +
                "<br><br>Please resolve the issue as soon as possible.<br><br>If the payment remains unsuccessful after {3} more day{4}, the amount will be deferred and a late fee of ${5:f2} will be added.",
                t.DateCharged, t.AmountCharged, t.ReasonFailed, daysRemaining, daysRemaining == 1 ? "" : "s", TuitionUtil.LATE_FEE);
            return GenerateEmail(user.Email, subject, rawBody, user.FirstName);
        }

        //Generates email notification for an unsuccessful payment that has been deferred
        public static EmailNotification PaymentFailedNotification(Transaction t, User user)
        {
            if (t == null)
            {
                throw new ArgumentNullException("Transaction cannot be null.");
            }
            if (user == null)
            {
                throw new ArgumentNullException("User cannot be null.");
            }

            string subject = "Alert from Tuition Assistant: Payment Failed";
            string rawBody = string.Format("Your payment of ${0:f2} that was due on {1:MMMM d yyyy} failed for {2} days and has been deferred." +
                "<br><br>The amount will be added to your next payment, along with a late fee of ${3:f2}.", t.AmountCharged, t.DateDue, TuitionUtil.OVERDUE_RETRY_PERIOD, TuitionUtil.LATE_FEE);
            return GenerateEmail(user.Email, subject, rawBody, user.FirstName);
        }

        //Generates email notification for updated account information
        public static EmailNotification AccountUpdatedNotification(string email, string firstName, string informationType)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("Email cannot be empty");
            }
            if (String.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException("First name cannot be empty");
            }
            if (String.IsNullOrEmpty(informationType))
            {
                throw new ArgumentNullException("Information type cannot be empty");
            }

            string subject = "Alert from Tuition Assistant: Account Information Updated";
            string rawbody = string.Format("Your {0} information was updated on your account. If you did not make this change, please contact your administrator.", informationType);
            return GenerateEmail(email, subject, rawbody, firstName);
        }

        //Generates email notification for account creation
        public static EmailNotification AccountCreatedNotification(User user, Transaction nextTransaction)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User cannot be null.");
            }
            if (nextTransaction == null)
            {
                throw new ArgumentNullException("Next transaction cannot be null.");
            }

            string subject = "Welcome to Tuition Assistant!";
            string rawbody = string.Format("Thank you creating an account with Tuition Assistant. We're glad you're here!<br><br>" +
                "Your next automatic payment will be on {0:MMMM d yyyy}, for an amount of ${1:f2}.<br>" +
                "We'll let you know five days before we charge your account.", nextTransaction.DateDue, nextTransaction.AmountCharged);
            return GenerateEmail(user.Email, subject, rawbody, user.FirstName);
        }

        //Generates email notification with the default Tuition Assistant body template
        public static EmailNotification GenerateEmail(string to, string subject, string rawBody, string userFirstName)
        {
            if (String.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException("The 'to' field cannot be null");
            }
            if (String.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException("The 'subject' field cannot be null");
            }
            if (String.IsNullOrEmpty(rawBody))
            {
                throw new ArgumentNullException("The 'rawbody' field cannot be null");
            }
            if (String.IsNullOrEmpty(userFirstName))
            {
                throw new ArgumentNullException("The 'userFirstName' field cannot be null");
            }
            string body = string.Format("Hi {0},<br><br>{1}<br>Please contact us if you have any questions.<br><br><br>Powered by Tuition Assistant<br>", userFirstName, rawBody);
            return new EmailNotification()
            {
                To = to,
                Subject = subject,
                Body = body
            };
        }
    }
}
