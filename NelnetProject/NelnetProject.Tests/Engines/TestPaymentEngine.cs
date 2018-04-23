using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;
using Engines;
using System.Diagnostics;
using Core.DTOs;

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
        MockGetTransactionAccessor getTransactionAccessor;

        public static List<Student> StudentsDB = new List<Student>()
        {
            new Student ()
            {
                StudentID = 1,
                FirstName = "Joe",
                LastName = "Sheepman",
                Grade = 8
            },
            new Student ()
            {
                StudentID = 2,
                FirstName = "Bill",
                LastName = "Billman",
                Grade = 11
            },
            new Student ()
            {
                StudentID = 3,
                FirstName = "Jeff",
                LastName = "Snaikes",
                Grade = 2
            }
        };
        public static List<User> MockUsersDB = new List<User>()
        {
            new User()
            {
                UserID = 1,
                FirstName = "John",
                LastName = "Smith",
                Email = "johnsmith@gmail.com",
                Hashed = "78b10e2cb3ec22bffea25bad2a1c02cbe4b7b587b46d0dd8d6af1c170910a3b1",
                Salt = "l1u2c3a4s5",
                Plan = PaymentPlan.MONTHLY,
                UserType = UserType.GENERAL,
                CustomerID = "fed123",
                Students = new List<Student>() { StudentsDB[0], StudentsDB[1] }
            },
            new User()
            {
                UserID = 2,
                FirstName = "Lucas",
                LastName = "Hall",
                Email = "lukethehallway@hall.mail",
                Hashed = "57855c02c995371dd1122a4b1ed2254a69d1ac3a9fe5d9c18676f9f6625bc5bb",
                Salt = "adfasfgth",
                Plan = PaymentPlan.SEMESTERLY,
                UserType = UserType.GENERAL,
                CustomerID = "123nonono",
                Students = new List<Student>() { StudentsDB[2] }
            }
        };
        public static List<UserPaymentInfoDTO> MockPaymentSpring = new List<UserPaymentInfoDTO>()
        {
            new UserPaymentInfoDTO()
            {
                CustomerID = "fed123",
                FirstName = "John",
                LastName = "Smith",
                StreetAddress1 = "1223 West St",
                StreetAddress2 = "Apt. 3",
                City = "Missouri City",
                State = "Kansas",
                Zip = "67208",
                CardNumber = 1123,
                ExpirationYear = 2025,
                ExpirationMonth = 6,
                CardType = "visa"
            }
        };

        private List<Transaction> TransactionDB = new List<Transaction>{
            new Transaction()
            {
                TransactionID = 2,
                UserID = 2,
                AmountCharged = 64.00,
                DateDue = new DateTime(2018, 2, 9),
                DateCharged = new DateTime(2018, 2, 9),
                ProcessState = ProcessState.SUCCESSFUL
            },
            new Transaction()
            {
                TransactionID = 3,
                UserID = 1,
                AmountCharged = 55.00,
                DateDue = new DateTime(2018, 2, 9),
                DateCharged = null,
                ProcessState = ProcessState.FAILED,
                ReasonFailed = "Insufficient funds"
            }
        };

        public TestPaymentEngine()
        {
            getUserInfoAccessor = new MockGetUserInfoAccessor(StudentsDB, MockUsersDB);
            getPaymentInfoAccessor = new MockGetPaymentInfoAccessor(MockPaymentSpring);
            chargePaymentAccessor = new MockChargePaymentAccessor();
            setTransactionAccessor = new MockSetTransactionAccessor();
            getTransactionAccessor = new MockGetTransactionAccessor(TransactionDB);
            paymentEngine = new PaymentEngine(getUserInfoAccessor, getPaymentInfoAccessor, chargePaymentAccessor, setTransactionAccessor, getTransactionAccessor);
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
                    AmountCharged = 875 + 55 + 25 + .75,
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

            expectedTransactions.AddRange(TransactionDB.Where(t => t.ProcessState == ProcessState.DEFERRED));
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
                    AmountCharged = 875,
                    DateDue = chargeDate,
                    ProcessState = ProcessState.NOT_YET_CHARGED
                },
                new Transaction
                {
                    TransactionID = 2,
                    UserID = 2,
                    AmountCharged = 1250,
                    DateDue = chargeDate,
                    ProcessState = ProcessState.NOT_YET_CHARGED
                }
            };

            List<Transaction> expectedTransactions = new List<Transaction>
            {
                new Transaction
                {
                    TransactionID = 1,
                    UserID = 1,
                    AmountCharged = 875,
                    DateDue = chargeDate,
                    DateCharged = chargeDate,
                    ProcessState = ProcessState.SUCCESSFUL
                },
                new Transaction
                {
                    TransactionID = 2,
                    UserID = 2,
                    AmountCharged = 1250,
                    DateDue = chargeDate,
                    DateCharged = chargeDate,
                    ProcessState = ProcessState.RETRYING,
                    ReasonFailed = "Insufficient Funds"
                }
            };

            chargePaymentAccessor.MockPaymentSpring.Add("fed123", new ChargeResultDTO()
            {
                WasSuccessful = true
            });
            chargePaymentAccessor.MockPaymentSpring.Add("123nonono", new ChargeResultDTO()
            {
                WasSuccessful = false,
                ErrorMessage = "Insufficient Funds"
            });

            List<Transaction> resultTransaction = paymentEngine.ChargePayments(inputTransactions, chargeDate).ToList();

            CollectionAssert.AreEqual(expectedTransactions, resultTransaction);
        }

        [TestMethod]
        public void TestChargePaymentsExpires()
        {
            DateTime chargeDate = new DateTime(2018, 9, 12);
            List<Transaction> inputTransactions = new List<Transaction>
            {
                new Transaction
                {
                    TransactionID = 1,
                    UserID = 1,
                    AmountCharged = 875,
                    DateDue = new DateTime(2018, 9, 5),
                    DateCharged = new DateTime(2018, 9, 11),
                    ProcessState = ProcessState.RETRYING
                }
            };

            List<Transaction> expectedTransactions = new List<Transaction>
            {
                new Transaction
                {
                    TransactionID = 1,
                    UserID = 1,
                    AmountCharged = 875,
                    DateDue = new DateTime(2018, 9, 5),
                    DateCharged = chargeDate,
                    ProcessState = ProcessState.FAILED,
                    ReasonFailed = "Card Expired"
                }
            };

            chargePaymentAccessor.MockPaymentSpring.Add("fed123", new ChargeResultDTO()
            {
                WasSuccessful = false,
                ErrorMessage = "Card Expired"
            });

            List<Transaction> resultTransaction = paymentEngine.ChargePayments(inputTransactions, chargeDate).ToList();

            CollectionAssert.AreEqual(expectedTransactions, resultTransaction);
        }

        [TestMethod]
        public void TestCalculatePaymentForNextUser()
        {
            int userId = 1;
            DateTime today = new DateTime(2018, 4, 15);
            Transaction expected = new Transaction()
            {
                UserID = 1,
                AmountCharged = 875,
                DateDue = new DateTime(2018, 5, 5),
                ProcessState = ProcessState.NOT_YET_CHARGED
            };

            Transaction actual = paymentEngine.CalculateNextPaymentForUser(userId, today);

            Assert.AreEqual(expected, actual);
        }
    }
}
