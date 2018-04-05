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
    }
}
