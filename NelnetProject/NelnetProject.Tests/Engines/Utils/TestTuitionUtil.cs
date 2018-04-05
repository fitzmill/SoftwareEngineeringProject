using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Engines.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NelnetProject.Tests.Engines.Utils
{
    [TestClass]
    public class TestTuitionUtil
    {
        private User BuildTestUser(PaymentPlan paymentPlan)
        {
            return new User
            {
                Plan = paymentPlan,
                Students = new List<int>() { 0, 5, 8, 12 }.Select(g => new Student { Grade = g }).ToList()
            };
        }

        [TestMethod]
        public void TestIsPaymentDueMonthlyYes()
        {
            DateTime date = new DateTime(2018, 12, 5);
            Assert.IsTrue(TuitionUtil.IsPaymentDue(PaymentPlan.MONTHLY, date));
        }

        [TestMethod]
        public void TestIsPaymentDueMonthlyNo()
        {
            DateTime date = new DateTime(2018, 6, 5);
            Assert.IsFalse(TuitionUtil.IsPaymentDue(PaymentPlan.MONTHLY, date));
        }

        [TestMethod]
        public void TestIsPaymentDueSemesterlyYes()
        {
            DateTime date = new DateTime(2018, 9, 5);
            Assert.IsTrue(TuitionUtil.IsPaymentDue(PaymentPlan.SEMESTERLY, date));
        }

        [TestMethod]
        public void TestIsPaymentDueSemesterlyNo()
        {
            DateTime date = new DateTime(2018, 12, 5);
            Assert.IsFalse(TuitionUtil.IsPaymentDue(PaymentPlan.SEMESTERLY, date));
        }

        [TestMethod]
        public void TestIsPaymentDueYearlyYes()
        {
            DateTime date = new DateTime(2018, 9, 5);
            Assert.IsTrue(TuitionUtil.IsPaymentDue(PaymentPlan.YEARLY, date));
        }

        [TestMethod]
        public void TestIsPaymentDueYearlyNo()
        {
            DateTime date = new DateTime(2018, 2, 5);
            Assert.IsFalse(TuitionUtil.IsPaymentDue(PaymentPlan.YEARLY, date));
        }

        [TestMethod]
        public void TestGenerateAmountDueMonthly()
        {
            User user = BuildTestUser(PaymentPlan.MONTHLY);
            double amountDue = TuitionUtil.GenerateAmountDue(user, 2);
            Assert.AreEqual(1375.00, amountDue);
        }

        [TestMethod]
        public void TestGenerateAmountDueSemesterly()
        {
            User user = BuildTestUser(PaymentPlan.SEMESTERLY);
            double amountDue = TuitionUtil.GenerateAmountDue(user, 2);
            Assert.AreEqual(6875.00, amountDue);
        }

        [TestMethod]
        public void TestGenerateAmountDueYearly()
        {
            User user = BuildTestUser(PaymentPlan.YEARLY);
            double amountDue = TuitionUtil.GenerateAmountDue(user, 2);
            Assert.AreEqual(13750, amountDue);
        }

        [TestMethod]
        public void TestNextPaymentDueDateMonthlyThisYearAfterDay()
        {
            PaymentPlan plan = PaymentPlan.MONTHLY;
            DateTime now = new DateTime(2018, 5, 13);

            DateTime expected = new DateTime(2018, 8, 5);
            DateTime actual = TuitionUtil.NextPaymentDueDate(plan, now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNextPaymentDueDateMonthlyNextYearAfterDay()
        {
            PaymentPlan plan = PaymentPlan.MONTHLY;
            DateTime now = new DateTime(2018, 12, 31);

            DateTime expected = new DateTime(2019, 1, 5);
            DateTime actual = TuitionUtil.NextPaymentDueDate(plan, now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNextPaymentDueDateMonthlyBeforeDay()
        {
            PaymentPlan plan = PaymentPlan.MONTHLY;
            DateTime now = new DateTime(2018, 9, 3);

            DateTime expected = new DateTime(2018, 9, 5);
            DateTime actual = TuitionUtil.NextPaymentDueDate(plan, now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNextPaymentDueDateSemesterlyThisYearAfterDay()
        {
            PaymentPlan plan = PaymentPlan.SEMESTERLY;
            DateTime now = new DateTime(2018, 2, 13);

            DateTime expected = new DateTime(2018, 9, 5);
            DateTime actual = TuitionUtil.NextPaymentDueDate(plan, now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNextPaymentDueDateSemesterlyNextYearAfterDay()
        {
            PaymentPlan plan = PaymentPlan.SEMESTERLY;
            DateTime now = new DateTime(2018, 11, 16);

            DateTime expected = new DateTime(2019, 2, 5);
            DateTime actual = TuitionUtil.NextPaymentDueDate(plan, now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNextPaymentDueDateSemesterlyBeforeDay()
        {
            PaymentPlan plan = PaymentPlan.SEMESTERLY;
            DateTime now = new DateTime(2018, 9, 3);

            DateTime expected = new DateTime(2018, 9, 5);
            DateTime actual = TuitionUtil.NextPaymentDueDate(plan, now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNextPaymentDueDateYearlyThisYearPreviousMonth()
        {
            PaymentPlan plan = PaymentPlan.YEARLY;
            DateTime now = new DateTime(2018, 2, 13);

            DateTime expected = new DateTime(2018, 9, 5);
            DateTime actual = TuitionUtil.NextPaymentDueDate(plan, now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNextPaymentDueDateYearlyNextYearAfterDay()
        {
            PaymentPlan plan = PaymentPlan.YEARLY;
            DateTime now = new DateTime(2018, 9, 16);

            DateTime expected = new DateTime(2019, 9, 5);
            DateTime actual = TuitionUtil.NextPaymentDueDate(plan, now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNextPaymentDueDateYearlyToday()
        {
            PaymentPlan plan = PaymentPlan.YEARLY;
            DateTime now = new DateTime(2018, 9, 5);

            DateTime expected = new DateTime(2018, 9, 5);
            DateTime actual = TuitionUtil.NextPaymentDueDate(plan, now);

            Assert.AreEqual(expected, actual);
        }
    }
}
