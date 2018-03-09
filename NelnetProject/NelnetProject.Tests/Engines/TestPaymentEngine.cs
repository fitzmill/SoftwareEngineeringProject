using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;
using Engines;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestPaymentEngine
    {
        PaymentEngine paymentEngine;
        MockGetUserInfoAccessor getUserInfoAccessor;
        MockGetPaymentInfoAccessor getPaymentInfoAccessor;
        MockChargePaymentAccessor chargePaymentAccessor;
        MockSetTransactionAccessor setTransactionAccessor;

        public TestPaymentEngine()
        {
            getUserInfoAccessor = new MockGetUserInfoAccessor();
            getPaymentInfoAccessor = new MockGetPaymentInfoAccessor();
            chargePaymentAccessor = new MockChargePaymentAccessor();
            setTransactionAccessor = new MockSetTransactionAccessor();
            paymentEngine = new PaymentEngine(getUserInfoAccessor, getPaymentInfoAccessor, chargePaymentAccessor, setTransactionAccessor);
        }

        private User BuildTestUser(PaymentPlan paymentPlan)
        {
            return new User
            {
                Plan = paymentPlan,
                Students = new List<int>() { 0, 5, 8, 12 }.Select(g => new Student { Grade = g }).ToList()
            };
        }

        [TestMethod]
        public void TestGenerateAmountDueMonthly()
        {
            User user = BuildTestUser(PaymentPlan.MONTHLY);

            double amountDue = paymentEngine.GenerateAmountDue(user);

            Assert.AreEqual(1145.83, amountDue);
        }

        [TestMethod]
        public void TestGenerateAmountDueSemesterly()
        {
            User user = BuildTestUser(PaymentPlan.SEMESTERLY);

            double amountDue = paymentEngine.GenerateAmountDue(user);

            Assert.AreEqual(6875, amountDue);
        }

        [TestMethod]
        public void TestGenerateAmountDueYearly()
        {
            User user = BuildTestUser(PaymentPlan.YEARLY);

            double amountDue = paymentEngine.GenerateAmountDue(user);

            Assert.AreEqual(13750, amountDue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Grade out of bounds: 13")]
        public void TestGenerateAmountExceptionThrown()
        {
            User user = BuildTestUser(PaymentPlan.MONTHLY);
            user.Students[0].Grade = 13;

            paymentEngine.GenerateAmountDue(user);
        }
    }
}
