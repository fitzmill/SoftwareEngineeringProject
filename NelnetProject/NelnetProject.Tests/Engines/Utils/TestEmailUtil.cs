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
            String expectedBody = "Hi Bob,\nThis is a test line.\nPlease contact us if you have any questions.\nPowered by Tuition Assistant\n";

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
            String expectedBody = "Hi Bob,\nYou have an upcoming payment.\nDate: March 30 2018\nAmount: $37.34\nYou don't need to worry about anything. We'll charge your card automatically on this date.\nPlease contact us if you have any questions.\nPowered by Tuition Assistant\n";

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
            String expectedBody = "Hi Bob,\nCongratulations! Your payment was processed succesfully.\nDate: March 3 2018\nAmount: $37.00\nPlease contact us if you have any questions.\nPowered by Tuition Assistant\n";

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
            String expectedBody = "Hi Bob,\nThere was an issue with your credit card. Your payment on April 3 2018 for $234.10 failed for the following reason: Card Expired." +
                "\nPlease resolve the issue as soon as possible.\nIf the payment remains unsuccessful after 3 more days, the amount will be deferred and a late fee of $25.00 will be added." +
                "\nPlease contact us if you have any questions.\nPowered by Tuition Assistant\n";

            EmailNotification email = EmailUtil.PaymentUnsuccessfulRetryingNotification(t, u, new DateTime(2018, 4, 3));

            Assert.AreEqual(u.Email, email.To);
            Assert.AreEqual("Alert from Tuition Assistant: Payment Unsuccessful (3 DAYS REMAINING)", email.Subject);
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

            String expectedBody = "Hi Bob,\nYour payment of $234.10 that was due on April 1 2018 failed for 7 days and has been deferred." +
                "\nThe amount will be added to your next payment, along with a late fee of $25.00.\nPlease contact us if you have any questions.\nPowered by Tuition Assistant\n";

            EmailNotification email = EmailUtil.PaymentFailedNotification(t, u);

            Assert.AreEqual(u.Email, email.To);
            Assert.AreEqual("Alert from Tuition Assistant: Payment Failed", email.Subject);
            Assert.AreEqual(expectedBody, email.Body);
        }

    }
}
