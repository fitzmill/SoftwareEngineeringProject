using System;
using Core;
using Core.Models;
using Engines.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NelnetProject.Tests.Engines.Utils
{
    [TestClass]
    public class TestEmailUtil
    {
        [TestMethod]
        public void TestGenerateEmail()
        {
            String to = "to@email";
            String subject = "subject";
            String rawBody = "This is a test line.";
            String userFirstName = "Bob";
            String expectedBody = "Hi Bob,<br><br>This is a test line.<br>Please contact us if you have any questions.<br><br><br>Powered by Tuition Assistant<br>";

            EmailNotification email = EmailUtil.GenerateEmail(to, subject, rawBody, userFirstName);

            Assert.AreEqual(to, email.To);
            Assert.AreEqual(subject, email.Subject);
            Assert.AreEqual(expectedBody, email.Body);
        }

        [TestMethod]
        public void TestUpcomingPaymentNotification()
        {
            Transaction t = new Transaction() {
                AmountCharged = 37.34,
                DateDue = new DateTime(2018, 3, 30)
            };
            User u = new User() {
                FirstName = "Bob",
                Email = "bob@email.com"
            };
            String expectedBody = "Hi Bob,<br><br>You have an upcoming payment.<br><br>Date: March 30 2018<br>Amount: $37.34" +
                "<br><br>You don't need to worry about anything. We'll charge your card automatically on this date.<br>" +
                "Please contact us if you have any questions.<br><br><br>Powered by Tuition Assistant<br>";

            EmailNotification email = EmailUtil.UpcomingPaymentNotification(t, u);

            Assert.AreEqual(u.Email, email.To);
            Assert.AreEqual("Alert from Tuition Assistant: Upcoming Payment", email.Subject);
            Assert.AreEqual(expectedBody, email.Body);
        }

        [TestMethod]
        public void TestPaymentChargedSuccessfullyNotification()
        {
            Transaction t = new Transaction()
            {
                AmountCharged = 37,
                DateDue = new DateTime(2018, 3, 1),
                DateCharged = new DateTime(2018, 3, 3),
                ProcessState = ProcessState.SUCCESSFUL
            };
            User u = new User()
            {
                FirstName = "Bob",
                Email = "bob@email.com"
            };
            String expectedBody = "Hi Bob,<br><br>Congratulations! Your payment was processed succesfully.<br><br>Date: March 3 2018" +
                "<br><br>Amount: $37.00<br>Please contact us if you have any questions.<br><br><br>Powered by Tuition Assistant<br>";

            EmailNotification email = EmailUtil.PaymentChargedSuccessfullyNotification(t, u);

            Assert.AreEqual(u.Email, email.To);
            Assert.AreEqual("Alert from Tuition Assistant: Payment Successful", email.Subject);
            Assert.AreEqual(expectedBody, email.Body);
        }

        [TestMethod]
        public void TestPaymentUnsuccessfulRetryingNotification()
        {
            Transaction t = new Transaction()
            {
                AmountCharged = 234.1,
                DateDue = new DateTime(2018, 4, 1),
                DateCharged = new DateTime(2018, 4, 3),
                ProcessState = ProcessState.RETRYING,
                ReasonFailed = "Card Expired"
            };
            User u = new User()
            {
                FirstName = "Bob",
                Email = "bob@email.com"
            };
            String expectedBody = "Hi Bob,<br><br>There was an issue with your credit card. Your payment on April 3 2018 for $234.10 failed for the following reason: Card Expired<br><br>" +
                "Please resolve the issue as soon as possible.<br><br>If the payment remains unsuccessful after 5 more days, the amount will be deferred and a late fee of $25.75 will be added.<br>" +
                "Please contact us if you have any questions.<br><br><br>Powered by Tuition Assistant<br>";

            EmailNotification email = EmailUtil.PaymentUnsuccessfulRetryingNotification(t, u, new DateTime(2018, 4, 3));

            Assert.AreEqual(u.Email, email.To);
            Assert.AreEqual("Alert from Tuition Assistant: Payment Unsuccessful (5 DAYS REMAINING)", email.Subject);
            Assert.AreEqual(expectedBody, email.Body);
        }

        [TestMethod]
        public void TestPaymentFailedNotification()
        {
            Transaction t = new Transaction()
            {
                AmountCharged = 234.1,
                DateDue = new DateTime(2018, 4, 1),
                DateCharged = new DateTime(2018, 4, 6),
                ProcessState = ProcessState.FAILED,
                ReasonFailed = "Card Expired"
            };
            User u = new User()
            {
                FirstName = "Bob",
                Email = "bob@email.com"
            };

            String expectedBody = "Hi Bob,<br><br>Your payment of $234.10 that was due on April 1 2018 failed for 7 days and has been deferred.<br><br>" +
                "The amount will be added to your next payment, along with a late fee of $25.75.<br>Please contact us if you have any questions." +
                "<br><br><br>Powered by Tuition Assistant<br>";

            EmailNotification email = EmailUtil.PaymentFailedNotification(t, u);

            Assert.AreEqual(u.Email, email.To);
            Assert.AreEqual("Alert from Tuition Assistant: Payment Failed", email.Subject);
            Assert.AreEqual(expectedBody, email.Body);
        }

        [TestMethod]
        public void TestAccountUpdatedNotification()
        {
            var email = "hi@me.com";
            var firstName = "Sean";
            var expected = new EmailNotification()
            {
                To = email,
                Subject = "Alert from Tuition Assistant: Account Information Updated",
                Body = "Hi Sean,<br><br>Your personal information was updated on your account. If you did not make this change, " +
                "please contact your administrator.<br>Please contact us if you have any questions." +
                "<br><br><br>Powered by Tuition Assistant<br>"
            };
            
            var result = EmailUtil.AccountUpdatedNotification(email, firstName, "personal");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Email cannot be empty")]
        public void TestAccountUpdatedNotificationNullTo()
        {
            var firstName = "Sean";
            EmailUtil.AccountUpdatedNotification(null, firstName, "personal");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "First name cannot be empty")]
        public void TestAccountUpdatedNotificationNullFirstName()
        {
            var email = "hi@me.com";
            EmailUtil.AccountUpdatedNotification(email, null, "personal");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Information type cannot be empty")]
        public void TestAccountUpdatedNotificationNullInfoType()
        {
            var email = "hi@me.com";
            var firstName = "Sean";
            EmailUtil.AccountUpdatedNotification(email, firstName, null);
        }

        [TestMethod]
        public void TestAccountCreatedNotification()
        {
            User user = new User()
            {
                FirstName = "Joe",
                Email = "joe@joe.r.accountant"
            };
            Transaction nextTransaction = new Transaction()
            {
                DateDue = new DateTime(2018, 9, 5),
                AmountCharged = 3125
            };

            var expected = new EmailNotification()
            {
                To = user.Email,
                Subject = "Welcome to Tuition Assistant!",
                Body = "Hi Joe,<br><br>Thank you creating an account with Tuition Assistant. We're glad you're here!<br><br>" +
                "Your next automatic payment will be on September 5 2018, for an amount of $3125.00.<br>" +
                "We'll let you know five days before we charge your account.<br>" +
                "Please contact us if you have any questions.<br><br><br>Powered by Tuition Assistant<br>"
            };

            var result = EmailUtil.AccountCreatedNotification(user, nextTransaction);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "User cannot be null")]
        public void TestAccountCreatedNotificationNullUser()
        {
            Transaction nextTransaction = new Transaction()
            {
                DateDue = new DateTime(2018, 9, 5),
                AmountCharged = 3125
            };

            EmailUtil.AccountCreatedNotification(null, nextTransaction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Transaction cannot be null")]
        public void TestAccountCreatedNotificationNullTransaction()
        {
            User user = new User()
            {
                FirstName = "Joe",
                Email = "joe@joe.r.accountant"
            };
            EmailUtil.AccountCreatedNotification(user, null);
        }

    }
}
