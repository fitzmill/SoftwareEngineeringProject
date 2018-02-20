using Core;
using Accessors;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NelnetProject.Tests.Accessors
{
    [TestClass]
    public class TestSetUserInfoAccessor
    {
        ISetUserInfoAccessor setUserInfoAccessor;

        public TestSetUserInfoAccessor()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;
            setUserInfoAccessor = new SetUserInfoAccessor(connectionString);
        }

        [TestMethod]
        public void TestInsertPersonalInfo()
        {
            User user = new User()
            {
                UserID = -1,
                FirstName = "Amanda",
                LastName = "Garfield",
                Email = "amanda@gmail.com",
                Hashed = "hashthis",
                Salt = "extrasecurity",
                PaymentPlan = PaymentPlan.MONTHLY,
                UserType = UserType.GENERAL,
                CustomerID = "fakeid",
                Students = new List<Student>()
            };

            setUserInfoAccessor.InsertPersonalInfo(user);

            Assert.IsTrue(user.UserID >= 1);
        }
    }
}
