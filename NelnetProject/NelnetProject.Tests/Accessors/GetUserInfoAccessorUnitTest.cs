using System;
using System.Configuration;
using Core;
using Accessors;
using Core.DTOs;
using Web;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class GetUserInfoAccessorUnitTest
    {
        private IGetUserInfoAccessor getUserInfoAccessor;
        public GetUserInfoAccessorUnitTest()
        {
            this.getUserInfoAccessor = new GetUserInfoAccessor(ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString);
        }

        [TestMethod]
        public void GetUserInfoByID()
        {
            int userID = 1;
            string testFirstName = "Cooper";
            User responseUser = getUserInfoAccessor.GetUserInfoByID(userID);
            Assert.IsNotNull(responseUser);
            Assert.AreEqual(testFirstName, responseUser.FirstName);
        }

        [TestMethod]
        public void GetUserInfoByEmail()
        {
            string email = "billy@microsoft.com";
            string testFirstName = "Bill";
            User responseUser = getUserInfoAccessor.GetUserInfoByEmail(email);
            Assert.IsNotNull(responseUser);
            Assert.AreEqual(testFirstName, responseUser.FirstName);
        }

        [TestMethod]
        public void EmailExists()
        {
            string email = "sean@weebnation.com";
            Boolean response = getUserInfoAccessor.EmailExists(email);
            Assert.IsNotNull(response);
            Assert.AreEqual(true, response);
        }

        [TestMethod]
        public void GetUserPasswordInfo()
        {
            string email = "cooper@cooperknaak.dating";
            string testHashed = "notimplementedyet";
            PasswordDTO response = getUserInfoAccessor.GetUserPasswordInfo(email);
            Assert.IsNotNull(response);
            Assert.AreEqual(testHashed, response.Hashed);
        }

        [TestMethod]
        public void GetPaymentSpringCustomerID()
        {
            int userID = 2;
            string testCustomerID = "1edf63";
            string responseCustomerID = getUserInfoAccessor.GetPaymentSpringCustomerID(userID);
            Assert.IsNotNull(responseCustomerID);
            Assert.AreEqual(testCustomerID, responseCustomerID);
        }
    }
}
