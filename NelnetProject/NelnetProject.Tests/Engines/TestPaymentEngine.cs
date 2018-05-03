using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;
using Engines;
using Core.DTOs;
using Engines.Utils;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestPaymentEngine
    {
        private readonly PaymentEngine _paymentEngine;
        private readonly MockUserAccessor _userAccessor;
        private readonly MockStudentAccessor _studentAccessor;
        private readonly MockPaymentAccessor _paymentAccessor;
        private readonly MockTransactionAccessor _transactionAccessor;

        public static List<Student> StudentsDB = new List<Student>()
        {
            new Student
            {
                StudentID = 1,
                UserID = 1,
                FirstName = "Joe",
                LastName = "Sheepman",
                Grade = 8
            },
            new Student
            {
                StudentID = 2,
                UserID = 1,
                FirstName = "Bill",
                LastName = "Billman",
                Grade = 11
            },
            new Student
            {
                StudentID = 3,
                UserID = 2,
                FirstName = "Jeff",
                LastName = "Snaikes",
                Grade = 2
            }
        };
        public static List<User> MockUsersDB = new List<User>()
        {
            new User
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
            new User
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
            new UserPaymentInfoDTO
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

        //private List<Transaction> TransactionDB = 

        public TestPaymentEngine()
        {
            _userAccessor = new MockUserAccessor(MockUsersDB);
            _studentAccessor = new MockStudentAccessor(StudentsDB);
            _paymentAccessor = new MockPaymentAccessor(new Dictionary<string, ChargeResultDTO>(), MockPaymentSpring);
            _transactionAccessor = new MockTransactionAccessor(new List<Transaction>());
            _paymentEngine = new PaymentEngine(_userAccessor, _studentAccessor, _paymentAccessor, _transactionAccessor);
        }

        [TestInitialize]
        public void InitTest()
        {
            _transactionAccessor._transactions = new List<Transaction>{
                new Transaction
                {
                    TransactionID = 2,
                    UserID = 2,
                    AmountCharged = 64.00,
                    DateDue = new DateTime(2018, 2, 9),
                    DateCharged = new DateTime(2018, 2, 9),
                    ProcessState = ProcessState.SUCCESSFUL
                },
                new Transaction
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
            List<Transaction> expectedTransactionsReturned = new List<Transaction>
            {
                new Transaction
                {
                    UserID = 1,
                    AmountCharged = Math.Round(55 + TuitionUtil.LATE_FEE + 875 * TuitionUtil.PROCESSING_FEE, TuitionUtil.DEFAULT_PRECISION),
                    DateDue = new DateTime(2018, 9, 5),
                    ProcessState = ProcessState.NOT_YET_CHARGED
                },
                new Transaction
                {
                    UserID = 2,
                    AmountCharged = Math.Round(1250 * TuitionUtil.PROCESSING_FEE, TuitionUtil.DEFAULT_PRECISION),
                    DateDue = new DateTime(2018, 9, 5),
                    ProcessState = ProcessState.NOT_YET_CHARGED
                }
            };

            List<Transaction> expectedTransactionsInDB = new List<Transaction>
            {
                new Transaction
                {
                    TransactionID = 2,
                    UserID = 2,
                    AmountCharged = 64.00,
                    DateDue = new DateTime(2018, 2, 9),
                    DateCharged = new DateTime(2018, 2, 9),
                    ProcessState = ProcessState.SUCCESSFUL
                },
                new Transaction
                {
                    UserID = 1,
                    AmountCharged = Math.Round(55 + TuitionUtil.LATE_FEE + 875 * TuitionUtil.PROCESSING_FEE, TuitionUtil.DEFAULT_PRECISION),
                    DateDue = new DateTime(2018, 9, 5),
                    ProcessState = ProcessState.NOT_YET_CHARGED
                },
                new Transaction
                {
                    UserID = 2,
                    AmountCharged = Math.Round(1250 * TuitionUtil.PROCESSING_FEE, TuitionUtil.DEFAULT_PRECISION),
                    DateDue = new DateTime(2018, 9, 5),
                    ProcessState = ProcessState.NOT_YET_CHARGED
                },
                new Transaction
                {
                    TransactionID = 3,
                    UserID = 1,
                    AmountCharged = 55.00,
                    DateDue = new DateTime(2018, 2, 9),
                    DateCharged = null,
                    ProcessState = ProcessState.DEFERRED,
                    ReasonFailed = "Insufficient funds"
                }
            };
            var users = _userAccessor.GetAllActiveUsers();
            foreach (User user in users)
            {
                user.Students = _studentAccessor.GetAllStudents().Where(x => x.UserID == user.UserID).ToList();
            }

            List<Transaction> result = _paymentEngine.GeneratePayments(users, genDate).ToList();

            CollectionAssert.AreEqual(expectedTransactionsReturned, result);
            CollectionAssert.AreEqual(expectedTransactionsInDB, _transactionAccessor._transactions.ToList());
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

            _paymentAccessor.MockPaymentSpringCharges.Add("fed123", new ChargeResultDTO()
            {
                WasSuccessful = true
            });
            _paymentAccessor.MockPaymentSpringCharges.Add("123nonono", new ChargeResultDTO()
            {
                WasSuccessful = false,
                ErrorMessage = "Insufficient Funds"
            });

            List<Transaction> resultTransaction = _paymentEngine.ChargePayments(inputTransactions, chargeDate).ToList();

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

            _paymentAccessor.MockPaymentSpringCharges.Add("fed123", new ChargeResultDTO()
            {
                WasSuccessful = false,
                ErrorMessage = "Card Expired"
            });

            List<Transaction> resultTransaction = _paymentEngine.ChargePayments(inputTransactions, chargeDate).ToList();

            CollectionAssert.AreEqual(expectedTransactions, resultTransaction);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "charge cannot be null")]
        public void TestChargePaymentsNullTransactions()
        {
            _paymentEngine.ChargePayments(new List<Transaction>() { null }, default(DateTime));
        }

        [TestMethod]
        public void TestCalculatePaymentForNextUserWithOverdue()
        {
            int userId = 1;
            DateTime today = new DateTime(2018, 4, 15);
            Transaction expected = new Transaction()
            {
                UserID = 1,
                AmountCharged = Math.Round(982.00, TuitionUtil.DEFAULT_PRECISION),
                DateDue = new DateTime(2018, 5, 5),
                ProcessState = ProcessState.NOT_YET_CHARGED
            };

            Transaction actual = _paymentEngine.CalculateNextPaymentForUser(userId, today);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCalculatePaymentForNextUserNotOverdue()
        {
            int userId = 2;
            DateTime today = new DateTime(2018, 4, 15);
            Transaction expected = new Transaction()
            {
                UserID = 2,
                AmountCharged = Math.Round(1287.50, TuitionUtil.DEFAULT_PRECISION),
                DateDue = new DateTime(2018, 9, 5),
                ProcessState = ProcessState.NOT_YET_CHARGED
            };

            Transaction actual = _paymentEngine.CalculateNextPaymentForUser(userId, today);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "User Id cannot be negative")]
        public void TestCalculateNextPaymentForUser()
        {
            int userId = -1;

            _paymentEngine.CalculateNextPaymentForUser(userId, default(DateTime));
        }

        [TestMethod]
        public void TestCalculatePeriodicPaymentMonthly()
        {
            User user = BuildTestUser(PaymentPlan.MONTHLY);
            double amountDue = _paymentEngine.CalculatePeriodicPayment(user);
            Assert.AreEqual(Math.Round(1375 * TuitionUtil.PROCESSING_FEE, TuitionUtil.DEFAULT_PRECISION), amountDue);
        }

        [TestMethod]
        public void TestCalculatePeriodicPaymentSemesterly()
        {
            User user = BuildTestUser(PaymentPlan.SEMESTERLY);
            double amountDue = _paymentEngine.CalculatePeriodicPayment(user);
            Assert.AreEqual(Math.Round(6875 * TuitionUtil.PROCESSING_FEE, TuitionUtil.DEFAULT_PRECISION), amountDue);
        }

        [TestMethod]
        public void TestCalculatePeriodicPaymentYearly()
        {
            User user = BuildTestUser(PaymentPlan.YEARLY);
            double amountDue = _paymentEngine.CalculatePeriodicPayment(user);
            Assert.AreEqual(Math.Round(13750 * TuitionUtil.PROCESSING_FEE, TuitionUtil.DEFAULT_PRECISION), amountDue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "user cannot be null")]
        public void TestCalculatePeriodicPaymentNullUser()
        {
            _paymentEngine.CalculatePeriodicPayment(null);
        }

        [TestMethod]
        public void TestInsertPaymentInfo()
        {
            UserPaymentInfoDTO paymentInfo = new UserPaymentInfoDTO
            {
                CustomerID = "",
                FirstName = "Lucas",
                LastName = "Hall",
                StreetAddress1 = "911 NE Emergency Ln",
                StreetAddress2 = "",
                City = "Chicago",
                State = "IL",
                Zip = "60007",
                CardNumber = 111111111111,
                ExpirationYear = 20,
                ExpirationMonth = 12,
                CardType = "Visa"
            };

            _paymentEngine.InsertPaymentInfo(paymentInfo);

            Assert.AreEqual(paymentInfo, _paymentAccessor.MockPaymentSpringCustomers.Where(info => info.CustomerID == paymentInfo.CustomerID).ToList().ElementAt(0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "user payment info cannot be null")]
        public void TestInsertPaymentInfoNull()
        {
            _paymentEngine.InsertPaymentInfo(null);
        }

        [TestMethod]
        public void TestUpdatePaymentBillingInfo()
        {
            _paymentAccessor.MockPaymentSpringCustomers.Add(new UserPaymentInfoDTO()
            {
                CustomerID = "fedder",
                CardNumber = 4111111111111111,
                ExpirationMonth = 12,
                ExpirationYear = 18
            });

            PaymentAddressDTO paymentAddressInfo = new PaymentAddressDTO
            {
                CustomerID = "fedder",
                FirstName = "Bobby",
                LastName = "Bobton",
                StreetAddress1 = "123 NE Eastern Ln",
                StreetAddress2 = "",
                City = "Chicago",
                State = "IL",
                Zip = "60007"
            };

            _paymentEngine.UpdatePaymentBillingInfo(paymentAddressInfo);
            string customerID = "fedder";

            Assert.AreEqual(paymentAddressInfo.FirstName, _paymentAccessor.MockPaymentSpringCustomers.Where(x => x.CustomerID == customerID).FirstOrDefault().FirstName);
            _paymentAccessor.MockPaymentSpringCustomers.RemoveAll(dto => dto.CustomerID == "fedder");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "payment address info cannot be null")]
        public void TestUpdatePaymentBillingInfoNull()
        {
            _paymentEngine.UpdatePaymentBillingInfo(null);
        }

        [TestMethod]
        public void TestUpdatePaymentCardInfo()
        {
            _paymentAccessor.MockPaymentSpringCustomers.Add(new UserPaymentInfoDTO()
            {
                CustomerID = "fedder",
                FirstName = "Bobby",
                LastName = "Bobton",
                StreetAddress1 = "123 NE Eastern Ln",
                StreetAddress2 = "",
                City = "Chicago",
                State = "IL",
                Zip = "60007"
            });

            PaymentCardDTO paymentCardInfo = new PaymentCardDTO
            {
                CustomerID = "fedder",
                CardNumber = 1234567891011111,
                ExpirationMonth = 12,
                ExpirationYear = 22
            };

            _paymentEngine.UpdatePaymentCardInfo(paymentCardInfo);
            string customerID = "fedder";

            Assert.AreEqual(paymentCardInfo.CardNumber, _paymentAccessor.MockPaymentSpringCustomers.Where(x => x.CustomerID == customerID).FirstOrDefault().CardNumber);
            _paymentAccessor.MockPaymentSpringCustomers.RemoveAll(dto => dto.CustomerID == "fedder");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "payment card info cannot be null")]
        public void TestUpdatePaymentCardInfoNull()
        {
            _paymentEngine.UpdatePaymentCardInfo(null);
        }

        [TestMethod]
        public void TestDeletePaymentInfo()
        {
            UserPaymentInfoDTO paymentInfo = new UserPaymentInfoDTO
            {
                CustomerID = "hello9",
                FirstName = "Hobo",
                LastName = "Guy",
                StreetAddress1 = "567 NW Weastern Rd",
                StreetAddress2 = "",
                City = "Kansas City",
                State = "MO",
                Zip = "64086",
                CardNumber = 333333333,
                ExpirationYear = 22,
                ExpirationMonth = 5,
                CardType = "MasterCard"
            };

            _paymentAccessor.MockPaymentSpringCustomers.Add(paymentInfo);

            _paymentEngine.DeletePaymentInfo(paymentInfo.CustomerID);

            Assert.IsFalse(_paymentAccessor.MockPaymentSpringCustomers.Contains(paymentInfo));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "customer Id cannot be empty")]
        public void TestDeletePaymentInfoNull()
        {
            _paymentEngine.DeletePaymentInfo(null);
        }

        [TestMethod]
        public void TestGetPaymentInfoForUserWithCorrectCustomerID()
        {
            int userID = 1;
            string firstName = "John";
            Assert.AreEqual(firstName, _paymentEngine.GetPaymentInfoForUser(userID).FirstName);
        }

        [TestMethod]
        public void TestGetPaymentInfoForUserWithoutCorrectCustomerID()
        {
            int userID = 4;
            Assert.AreEqual(null, _paymentEngine.GetPaymentInfoForUser(userID));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "user id cannot be negative")]
        public void TestGetPaymentInfoForNegativeUserId()
        {
            _paymentEngine.GetPaymentInfoForUser(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "users cannot be null")]
        public void TestGeneratePaymentsUsersNull()
        {
            _paymentEngine.GeneratePayments(null, DateTime.Now);
        }
    }
}
