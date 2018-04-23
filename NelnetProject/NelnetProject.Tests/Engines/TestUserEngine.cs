using Core;
using Core.Interfaces.Accessors;
using Core.Interfaces.Engines;
using Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelnetProject.Tests.Engines.MockedAccessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines
{
    [TestClass]
    public class TestUserEngine
    {
        private readonly MockUserAccessor _userAccessor;
        private readonly MockStudentAccessor _studentAccessor;
        private readonly IUserEngine _userEngine;

        public List<Student> StudentsDB = new List<Student>()
        {
            new Student ()
            {
                StudentID = 1,
                UserID = 1,
                FirstName = "Joe",
                LastName = "Sheepman",
                Grade = 8
            },
            new Student ()
            {
                StudentID = 2,
                UserID = 1,
                FirstName = "Bill",
                LastName = "Billman",
                Grade = 11
            },
            new Student ()
            {
                StudentID = 3,
                UserID = 2,
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

        public TestUserEngine()
        {
            _userAccessor = new MockUserAccessor(MockUsersDB);
            _studentAccessor = new MockStudentAccessor(StudentsDB);
            _userEngine = new UserEngine(_userAccessor, _studentAccessor, new RNGCryptoServiceProvider());
        }

        [TestInitialize]
        public void InitializeUserEngineTests()
        {
            _userAccessor.MockDb = MockUsersDB;
            _studentAccessor.MockDb = StudentsDB;
        }

        [TestMethod]
        public void TestInsertPersonalInfoOneStudent()
        {
            User user = new User()
            {
                UserID = -1,
                FirstName = "Nail",
                LastName = "Hammer",
                Email = "hammerandnail@gmail.com",
                Hashed = "qwerty",
                Salt = "uiop",
                Plan = PaymentPlan.MONTHLY,
                UserType = UserType.GENERAL,
                CustomerID = "1j3k7r",
                Students = new List<Student>()
                {
                    new Student()
                    {
                        StudentID = -1,
                        FirstName = "William",
                        LastName = "Hammer",
                        Grade = 8
                    }
                }
            };

            _userEngine.InsertPersonalInfo(user, "Cornflakes*7");

            Assert.IsTrue(_userAccessor.MockDb.Contains(user));
            Assert.AreEqual(user, _userAccessor.MockDb.FirstOrDefault(u => u.UserID == user.UserID));
            Assert.IsTrue(user.UserID >= 0);
        }

        [TestMethod]
        public void TestUpdatePersonalInfoOneStudent()
        {
            User user = new User()
            {
                UserID = -1,
                FirstName = "Andrew",
                LastName = "Crewman",
                Email = "andrewcrewman@gmail.com",
                Hashed = "klsdfl",
                Salt = "fs8e",
                Plan = PaymentPlan.SEMESTERLY,
                UserType = UserType.GENERAL,
                CustomerID = "9wy84q",
                Students = new List<Student>()
            };

            _userAccessor.InsertPersonalInfo(user);

            user.FirstName = "Andy";
            user.Email = "andy@gmail.com";

            _userEngine.UpdatePersonalInfo(user);

            Assert.IsTrue(_userAccessor.MockDb.Contains(user));
            Assert.AreEqual(user, _userAccessor.MockDb.FirstOrDefault(u => u.UserID == user.UserID));
            Assert.IsTrue(user.UserID > 0);
        }

        [TestMethod]
        public void TestDeletePersonalInfo()
        {
            User user = new User()
            {
                UserID = -1,
                FirstName = "Emma",
                LastName = "Hannah",
                Email = "emma@gmail.com",
                Hashed = "jk65ui",
                Salt = "ke978n",
                Plan = PaymentPlan.MONTHLY,
                UserType = UserType.GENERAL,
                CustomerID = "77777g",
                Students = new List<Student>()
            };
            _userAccessor.InsertPersonalInfo(user);

            _userEngine.DeletePersonalInfo(user.UserID);

            Assert.IsFalse(_userAccessor.MockDb.Contains(user));
        }

        [TestMethod]
        public void TestEmailExistsTrue()
        {
            string email = "johnsmith@gmail.com";
            Assert.IsTrue(_userEngine.EmailExists(email));
        }

        [TestMethod]
        public void TestEmailExistsFalse()
        {
            string email = "thisisnotanemail@gmail.com";
            Assert.IsFalse(_userEngine.EmailExists(email));
        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            var expected = MockUsersDB;
            foreach(User user in expected)
            {
                user.Students = StudentsDB.Where(x => x.UserID == user.UserID);
            }
            CollectionAssert.AreEqual(expected, _userEngine.GetAllUsers().ToList());
        }

        [TestMethod]
        public void TestValidateLoginInformationFalse()
        {
            string email = "johnsmith@gmail.com";
            string password = "majlehlasg";
            Assert.IsNull(_userEngine.ValidateLoginInfo(email, password));
        }

        [TestMethod]
        public void TestValidateLoginInformationTrue()
        {
            string email = "johnsmith@gmail.com";
            string password = "password";
            Assert.AreEqual(_userAccessor.GetUserInfoByEmail(email), _userEngine.ValidateLoginInfo(email, password));
        }

        [TestMethod]
        public void TestGetUserInfoByIDWithCorrectID()
        {
            int userID = 1;
            string firstName = "John";
            Assert.AreEqual(firstName, _userEngine.GetUserInfoByID(userID).FirstName);
        }

        [TestMethod]
        public void TestGetUserInfoByIDWithoutCorrectID()
        {
            int userID = 23;
            Assert.AreEqual(null, _userEngine.GetUserInfoByID(userID));
        }
    }
}
