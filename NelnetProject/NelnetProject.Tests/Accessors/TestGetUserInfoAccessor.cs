using System;
using System.Configuration;
using Core;
using Accessors;
using Core.DTOs;
using Web;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestGetUserInfoAccessorUnit
    {
        private IGetUserInfoAccessor getUserInfoAccessor;
        public TestGetUserInfoAccessorUnit()
        {
            this.getUserInfoAccessor = new GetUserInfoAccessor(ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString);
        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            IList<User> users = getUserInfoAccessor.GetAllActiveUsers();
            Assert.IsNotNull(users[0]);
        }

        [TestMethod]
        public void TestGetUserInfoByID()
        {
            int userID = 1;
            User responseUser = getUserInfoAccessor.GetUserInfoByID(userID);
            Assert.IsNotNull(responseUser);
        }

        [TestMethod]
        public void TestGetUserInfoByEmail()
        {
            string email = "billy@microsoft.com";
            User responseUser = getUserInfoAccessor.GetUserInfoByEmail(email);
            Assert.IsNotNull(responseUser);
        }

        [TestMethod]
        public void TestEmailExists()
        {
            string email = "sean@weebnation.com";
            Boolean response = getUserInfoAccessor.EmailExists(email);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void TestGetUserPasswordInfo()
        {
            string email = "cooper@cooperknaak.dating";
            PasswordDTO response = getUserInfoAccessor.GetUserPasswordInfo(email);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void TestGetPaymentSpringCustomerID()
        {
            int userID = 2;
            string responseCustomerID = getUserInfoAccessor.GetPaymentSpringCustomerID(userID);
            Assert.IsNotNull(responseCustomerID);
        }
    }
}
