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
        public void GetUserInfoByIDTest()
        {
            int userID = 1;
            User responseUser = getUserInfoAccessor.GetUserInfoByID(userID);
            Assert.IsNotNull(responseUser);
        }

        [TestMethod]
        public void GetUserInfoByEmailTest()
        {
            string email = "billy@microsoft.com";
            User responseUser = getUserInfoAccessor.GetUserInfoByEmail(email);
            Assert.IsNotNull(responseUser);
        }

        [TestMethod]
        public void EmailExistsTest()
        {
            string email = "sean@weebnation.com";
            Boolean response = getUserInfoAccessor.EmailExists(email);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetUserPasswordInfoTest()
        {
            string email = "cooper@cooperknaak.dating";
            PasswordDTO response = getUserInfoAccessor.GetUserPasswordInfo(email);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetPaymentSpringCustomerIDTest()
        {
            int userID = 2;
            string responseCustomerID = getUserInfoAccessor.GetPaymentSpringCustomerID(userID);
            Assert.IsNotNull(responseCustomerID);
        }
    }
}
