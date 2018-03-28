using Core;
using Core.DTOs;
using Core.Interfaces;
using Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;
using System.Collections.Generic;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestGetUserInfoEngine
    {
        public List<Student> StudentsDB = new List<Student>()
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
        public List<User> MockUsersDB = new List<User>()
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
                Students = new List<Student>()
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
                Students = new List<Student>()
            }
        };
        public List<UserPaymentInfoDTO> MockPaymentSpring = new List<UserPaymentInfoDTO>()
        {
            new UserPaymentInfoDTO()
            {
                CustomerID = "fed123",
                Company = "Martwall",
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

        IGetUserInfoEngine getUserInfoEngine;
        IGetPaymentInfoAccessor getPaymentInfoAccessor;
        IGetUserInfoAccessor getUserInfoAccessor;
        public TestGetUserInfoEngine()
        {
            getPaymentInfoAccessor = new MockGetPaymentInfoAccessor(MockPaymentSpring);
            getUserInfoAccessor = new MockGetUserInfoAccessor(StudentsDB, MockUsersDB);
            getUserInfoEngine = new GetUserInfoEngine(getPaymentInfoAccessor, getUserInfoAccessor);
        }

        [TestMethod]
        public void TestEmailExistsTrue()
        {
            string email = "johnsmith@gmail.com";
            Assert.IsTrue(getUserInfoEngine.EmailExists(email));
        }

        [TestMethod]
        public void TestEmailExistsFalse()
        {
            string email = "thisisnotanemail@gmail.com";
            Assert.IsFalse(getUserInfoEngine.EmailExists(email));
        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            int expectedValue = 2;
            int inputIndex = 1;
            Assert.AreEqual(expectedValue, getUserInfoEngine.GetAllUsers()[inputIndex].UserID);
        }
        [TestMethod]
        public void TestValidateLoginInformationFalse()
        {
            string email = "johnsmith@gmail.com";
            string password = "majlehlasg";
            Assert.IsFalse(getUserInfoEngine.ValidateLoginInfo(email, password));
        }

        [TestMethod]
        public void TestValidateLoginInformationTrue()
        {
            string email = "johnsmith@gmail.com";
            string password = "password";
            Assert.IsTrue(getUserInfoEngine.ValidateLoginInfo(email, password));
        }

        [TestMethod]
        public void TestGetUserInfoByIDWithCorrectID()
        {
            int userID = 1;
            string firstName = "John";
            Assert.AreEqual(firstName, getUserInfoEngine.GetUserInfoByID(userID).FirstName);
        }

        [TestMethod]
        public void TestGetUserInfoByIDWithoutCorrectID()
        {
            int userID = 23;
            Assert.AreEqual(null, getUserInfoEngine.GetUserInfoByID(userID));
        }

        [TestMethod]
        public void TestGetUserInfoByEmailWithCorrectEmail()
        {
            string email = "johnsmith@gmail.com";
            string firstName = "John";
            Assert.AreEqual(firstName, getUserInfoEngine.GetUserInfoByEmail(email).FirstName);
        }

        [TestMethod]
        public void TestGetUserInfoByEmailWithoutCorrectEmail()
        {
            string email = "thisemailisntanemail@gmail.com";
            Assert.AreEqual(null, getUserInfoEngine.GetUserInfoByEmail(email));
        }

        [TestMethod]
        public void TestGetPaymentInfoForUserWithCorrectCustomerID()
        {
            int userID = 1;
            string firstName = "John";
            Assert.AreEqual(firstName, getUserInfoEngine.GetPaymentInfoForUser(userID).FirstName);
        }

        [TestMethod]
        public void TestGetPaymentInfoForUserWithoutCorrectCustomerID()
        {
            int userID = 4;
            Assert.AreEqual(null, getUserInfoEngine.GetPaymentInfoForUser(userID));
        }
    }
}
