using Accessors;
using Core;
using Core.Interfaces.Accessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestUserAccessor
    {
        private readonly string _connectionString;
        private readonly IUserAccessor _userAccessor;

        public TestUserAccessor()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;
            _userAccessor = new UserAccessor();
        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            IEnumerable<User> users = _userAccessor.GetAllActiveUsers();
            Assert.IsNotNull(users);
        }

        [TestMethod]
        public void TestGetUserInfoByID()
        {
            int userID = 1;
            User responseUser = _userAccessor.GetUserInfoByID(userID);
            Assert.IsNotNull(responseUser);
        }

        [TestMethod]
        public void TestGetUserInfoByEmail()
        {
            string email = "billy@microsoft.com";
            User responseUser = _userAccessor.GetUserInfoByEmail(email);
            Assert.IsNotNull(responseUser);
        }

        [TestMethod]
        public void TestEmailExists()
        {
            string email = "sean@weebnation.com";
            Boolean response = _userAccessor.EmailExists(email);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void TestGetPaymentSpringCustomerID()
        {
            int userID = 2;
            string responseCustomerID = _userAccessor.GetPaymentSpringCustomerID(userID);
            Assert.IsNotNull(responseCustomerID);
        }

        [TestMethod]
        public void TestConnection()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Close();
            }
        }
    }
}
