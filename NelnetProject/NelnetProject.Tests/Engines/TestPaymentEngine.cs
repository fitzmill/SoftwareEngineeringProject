using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;
using Engines;
using System.Diagnostics;

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
        public void TestGeneratePaymentsAllThisMonth()
        {
            DateTime genDate = new DateTime(2018, 9, 1);
            List<Transaction> expectedTransactions = new List<Transaction>
            {
                new Transaction
                {
                    UserID = 1,
                    AmountCharged = 729.17,
                    DateDue = new DateTime(2018, 9, 5),
                    ProcessState = ProcessState.NOT_YET_CHARGED
                },
                new Transaction
                {
                    UserID = 2,
                    AmountCharged = 1250,
                    DateDue = new DateTime(2018, 9, 5),
                    ProcessState = ProcessState.NOT_YET_CHARGED
                }
            };

            List<Transaction> result = paymentEngine.GeneratePayments(genDate).ToList();

            CollectionAssert.AreEqual(expectedTransactions, result);
            CollectionAssert.AreEqual(expectedTransactions, setTransactionAccessor.Transactions);
        }

        [TestMethod]
        public void TestChargePayments()
        {
            DateTime chargeDate = new DateTime(2018, 9, 5);
            List<Transaction> inputTransactions = new List<Transaction>
            {
                new Transaction
                {
                    TransactionID = 1,
                    UserID = 1,
                    AmountCharged = 729.17,
                    DateDue = new DateTime(2018, 9, 5),
                    ProcessState = ProcessState.NOT_YET_CHARGED
                },
                new Transaction
                {
                    TransactionID = 2,
                    UserID = 2,
                    AmountCharged = 1250,
                    DateDue = new DateTime(2018, 9, 5),
                    ProcessState = ProcessState.NOT_YET_CHARGED
                }
            };

            List<Transaction> expectedTransactions = new List<Transaction>
            {
                new Transaction
                {
                    TransactionID = 1,
                    UserID = 1,
                    AmountCharged = 729.17,
                    DateDue = new DateTime(2018, 9, 5),
                    ProcessState = ProcessState.SUCCESSFUL
                },
                new Transaction
                {
                    TransactionID = 2,
                    UserID = 2,
                    AmountCharged = 1250,
                    DateDue = new DateTime(2018, 9, 5),
                    ProcessState = ProcessState.RETRYING
                }
            };



            List<Transaction> resultTransaction = paymentEngine.ChargePayments(inputTransactions, chargeDate).ToList();


        }
    }
}
