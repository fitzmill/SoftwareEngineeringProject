using Core.Interfaces;
using Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestGetUserInfoEngine
    {
        IGetUserInfoEngine getUserInfoEngine;
        IGetPaymentInfoAccessor getPaymentInfoAccessor;
        IGetUserInfoAccessor getUserInfoAccessor;
        public TestGetUserInfoEngine()
        {
            getPaymentInfoAccessor = new MockGetPaymentInfoAccessor();
            getUserInfoAccessor = new MockGetUserInfoAccessor();
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
            string customerID = "fed123";
            string firstName = "John";
            Assert.AreEqual(firstName, getUserInfoEngine.GetPaymentInfoForUser(customerID).FirstName);
        }

        [TestMethod]
        public void TestGetPaymentInfoForUserWithoutCorrectCustomerID()
        {
            string customerID = "thisisnotvalid";
            Assert.AreEqual(null, getUserInfoEngine.GetPaymentInfoForUser(customerID));
        }
    }
}
