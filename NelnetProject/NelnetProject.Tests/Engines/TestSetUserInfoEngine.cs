using Core;
using Core.DTOs;
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
    public class TestSetUserInfoEngine
    {
        MockSetUserInfoAccessor setUserInfoAccessor;
        MockSetPaymentInfoAccessor setPaymentInfoAccessor;
        SetUserInfoEngine setUserInfoEngine;

        public List<Student> mockStudentTable = new List<Student>()
        {
            new Student()
            {
                StudentID = 1,
                FirstName = "Lucas",
                LastName = "Hall",
                Grade = 9
            },

            new Student()
            {
                StudentID = 2,
                FirstName = "Joe",
                LastName = "Cowboy",
                Grade = 4
            }
        };

        public List<User> mockUserTable = new List<User>()
        {
            new User()
            {
                UserID = 1,
                FirstName = "George",
                LastName = "Curious",
                Email = "curious.george@gmail.com",
                Hashed = "asdfjkl",
                Salt = "salt",
                Plan = PaymentPlan.MONTHLY,
                UserType = UserType.GENERAL,
                CustomerID = "abc123",
                Students = new List<Student>()
            }
        };

        public List<UserPaymentInfoDTO> mockPaymentSpring = new List<UserPaymentInfoDTO>
        {
            new UserPaymentInfoDTO
            {
                CustomerID = "abcdef",
                FirstName = "George",
                LastName = "Curious",
                StreetAddress1 = "601 NE Robin St",
                StreetAddress2 = "",
                City = "New York",
                State = "NY",
                Zip = "10001",
                CardNumber = 123412341234,
                ExpirationYear = 20,
                ExpirationMonth = 12,
                CardType = "Visa"
            }
        };

        public TestSetUserInfoEngine()
        {
            mockUserTable.ElementAt(0).Students.Add(mockStudentTable.ElementAt(0));
            mockUserTable.ElementAt(0).Students.Add(mockStudentTable.ElementAt(1));

            setUserInfoAccessor = new MockSetUserInfoAccessor(mockUserTable, mockStudentTable);
            setPaymentInfoAccessor = new MockSetPaymentInfoAccessor(mockPaymentSpring);
            setUserInfoEngine = new SetUserInfoEngine(setUserInfoAccessor, setPaymentInfoAccessor);
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

            setUserInfoEngine.InsertPaymentInfo(paymentInfo);

            Assert.IsTrue(setPaymentInfoAccessor.mockPaymentSpring.Contains(paymentInfo));
            Assert.AreEqual(paymentInfo, setPaymentInfoAccessor.mockPaymentSpring.Where(info => info.CustomerID == paymentInfo.CustomerID).ToList().ElementAt(0));
        }

        [TestMethod]
        public void TestUpdatePaymentBillingInfo()
        {
            setPaymentInfoAccessor.mockPaymentSpring.Add(new UserPaymentInfoDTO()
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

            setUserInfoEngine.UpdatePaymentBillingInfo(paymentAddressInfo);
            string customerID = "fedder";

            Assert.IsTrue(setPaymentInfoAccessor.mockPaymentSpring.Select(x => x.CustomerID).Contains(customerID));
            Assert.AreEqual(paymentAddressInfo.FirstName, setPaymentInfoAccessor.mockPaymentSpring.Where(x => x.CustomerID == customerID).FirstOrDefault().FirstName);
            setPaymentInfoAccessor.mockPaymentSpring.RemoveAll(dto => dto.CustomerID == "fedder");
        }

        [TestMethod]
        public void TestUpdatePaymentCardInfo()
        {
            setPaymentInfoAccessor.mockPaymentSpring.Add(new UserPaymentInfoDTO()
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

            setUserInfoEngine.UpdatePaymentCardInfo(paymentCardInfo);
            string customerID = "fedder";

            Assert.IsTrue(setPaymentInfoAccessor.mockPaymentSpring.Select(x => x.CustomerID).Contains(customerID));
            Assert.AreEqual(paymentCardInfo.CardNumber, setPaymentInfoAccessor.mockPaymentSpring.Where(x => x.CustomerID == customerID).FirstOrDefault().CardNumber);
            setPaymentInfoAccessor.mockPaymentSpring.RemoveAll(dto => dto.CustomerID == "fedder");
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

            setPaymentInfoAccessor.mockPaymentSpring.Add(paymentInfo);

            setUserInfoEngine.DeletePaymentInfo(paymentInfo.CustomerID);

            Assert.IsFalse(setPaymentInfoAccessor.mockPaymentSpring.Contains(paymentInfo));
        }

        [TestMethod]
        public void TestInsertPersonalInfoOneStudent()
        {
            Student student = new Student()
            {
                StudentID = -1,
                FirstName = "William",
                LastName = "Hammer",
                Grade = 8
            };
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
            };
            user.Students.Add(student);

            setUserInfoEngine.InsertPersonalInfo(user);

            Assert.IsTrue(setUserInfoAccessor.mockUserTable.Contains(user));
            Assert.AreEqual(user, setUserInfoAccessor.mockUserTable.Where(u => u.UserID == user.UserID).ToList().ElementAt(0));
            Assert.IsTrue(user.UserID > 0);
            foreach(Student s in user.Students)
            {
                Assert.IsTrue(setUserInfoAccessor.mockStudentTable.Contains(s));
                Assert.AreEqual(s, setUserInfoAccessor.mockStudentTable.Where(o => o.StudentID == s.StudentID).ToList().ElementAt(0));
                Assert.IsTrue(s.StudentID > 0);
            }
        }

        [TestMethod]
        public void TestInsertPersonalInfoMultipleStudents()
        {
            User user = new User()
            {
                UserID = -1,
                FirstName = "Sean",
                LastName = "Fitz",
                Email = "seanfitz@gmail.com",
                Hashed = "cool",
                Salt = "dude",
                Plan = PaymentPlan.SEMESTERLY,
                UserType = UserType.GENERAL,
                CustomerID = "34jk89",
                Students = new List<Student>()
            };
            Student student1 = new Student()
            {
                StudentID = -1,
                FirstName = "Sean Jr.",
                LastName = "Fitz",
                Grade = 8
            };
            Student student2 = new Student()
            {
                StudentID = -1,
                FirstName = "Sean III",
                LastName = "Fitz",
                Grade = 4
            };
            Student student3 = new Student()
            {
                StudentID = -1,
                FirstName = "Sean IV",
                LastName = "Fitz",
                Grade = 2
            };
            user.Students.Add(student1);
            user.Students.Add(student2);
            user.Students.Add(student3);

            setUserInfoEngine.InsertPersonalInfo(user);

            Assert.IsTrue(setUserInfoAccessor.mockUserTable.Contains(user));
            Assert.AreEqual(user, setUserInfoAccessor.mockUserTable.Where(u => u.UserID == user.UserID).ToList().ElementAt(0));
            Assert.IsTrue(user.UserID > 0);
            foreach (Student s in user.Students)
            {
                Assert.IsTrue(setUserInfoAccessor.mockStudentTable.Contains(s));
                Assert.AreEqual(s, setUserInfoAccessor.mockStudentTable.Where(o => o.StudentID == s.StudentID).ToList().ElementAt(0));
                Assert.IsTrue(s.StudentID > 0);
            }
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
            Student student = new Student()
            {
                StudentID = -1,
                FirstName = "Drew",
                LastName = "Crewman",
                Grade = 4
            };
            user.Students.Add(student);

            setUserInfoAccessor.InsertPersonalInfo(user);
            setUserInfoAccessor.InsertStudentInfo(user.UserID, student);

            student.FirstName = "Threw";
            student.Grade = 5;
            user.FirstName = "Andy";
            user.Email = "andy@gmail.com";

            setUserInfoEngine.UpdatePersonalInfo(user);

            Assert.IsTrue(setUserInfoAccessor.mockUserTable.Contains(user));
            Assert.AreEqual(user, setUserInfoAccessor.mockUserTable.Where(u => u.UserID == user.UserID).ToList().ElementAt(0));
            Assert.IsTrue(user.UserID > 0);
            foreach (Student s in user.Students)
            {
                Assert.IsTrue(setUserInfoAccessor.mockStudentTable.Contains(s));
                Assert.AreEqual(s, setUserInfoAccessor.mockStudentTable.Where(o => o.StudentID == s.StudentID).ToList().ElementAt(0));
                Assert.IsTrue(s.StudentID > 0);
            }
        }

        [TestMethod]
        public void TestUpdatePersonalInfoManyStudents()
        {
            User user = new User()
            {
                UserID = -1,
                FirstName = "Jake",
                LastName = "Lichtenhausen",
                Email = "lichtenhausen.jake@gmail.com",
                Hashed = "234klkj2",
                Salt = "9dr0w",
                Plan = PaymentPlan.MONTHLY,
                UserType = UserType.GENERAL,
                CustomerID = "jdie58",
                Students = new List<Student>()
            };
            Student student1 = new Student()
            {
                StudentID = -1,
                FirstName = "Andrew",
                LastName = "Lichtenhausen",
                Grade = 4
            };
            user.Students.Add(student1);
            Student student2 = new Student()
            {
                StudentID = -1,
                FirstName = "Greg",
                LastName = "Lichtenhausen",
                Grade = 6
            };
            user.Students.Add(student2);

            setUserInfoAccessor.InsertPersonalInfo(user);
            setUserInfoAccessor.InsertStudentInfo(user.UserID, student1);
            setUserInfoAccessor.InsertStudentInfo(user.UserID, student2);

            student1.FirstName = "Andy";
            student1.Grade = 5;
            student2.FirstName = "Gregory";
            student2.Grade = 7;
            user.FirstName = "Jacob";
            user.Email = "jakey@gmail.com";
            user.Plan = PaymentPlan.SEMESTERLY;

            setUserInfoEngine.UpdatePersonalInfo(user);

            Assert.IsTrue(setUserInfoAccessor.mockUserTable.Contains(user));
            Assert.AreEqual(user, setUserInfoAccessor.mockUserTable.Where(u => u.UserID == user.UserID).ToList().ElementAt(0));
            Assert.IsTrue(user.UserID > 0);
            foreach (Student s in user.Students)
            {
                Assert.IsTrue(setUserInfoAccessor.mockStudentTable.Contains(s));
                Assert.AreEqual(s, setUserInfoAccessor.mockStudentTable.Where(o => o.StudentID == s.StudentID).ToList().ElementAt(0));
                Assert.IsTrue(s.StudentID > 0);
            }
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
            Student student = new Student()
            {
                StudentID = -1,
                FirstName = "Man",
                LastName = "Hannah",
                Grade = 8
            };
            user.Students.Add(student);
            UserPaymentInfoDTO paymentInfo = new UserPaymentInfoDTO
            {
                CustomerID = "77777g",
                FirstName = "Emma",
                LastName = "Hannah",
                StreetAddress1 = "8732 W George Ct",
                StreetAddress2 = "",
                City = "Chicago",
                State = "IL",
                Zip = "60007",
                CardNumber = 1111222233334444,
                ExpirationYear = 20,
                ExpirationMonth = 8,
                CardType = "Mastercard"
            };

            setPaymentInfoAccessor.mockPaymentSpring.Add(paymentInfo);
            setUserInfoAccessor.InsertPersonalInfo(user);
            setUserInfoAccessor.InsertStudentInfo(user.UserID, student);

            setUserInfoEngine.DeletePersonalInfo(user.UserID, user.CustomerID);

            Assert.IsFalse(setPaymentInfoAccessor.mockPaymentSpring.Contains(paymentInfo));
            Assert.IsFalse(setUserInfoAccessor.mockStudentTable.Contains(student));
            Assert.IsFalse(setUserInfoAccessor.mockUserTable.Contains(user));
        }

        [TestMethod]
        public void TestInsertStudentInfo()
        {
            User user = new User()
            {
                UserID = 30,
                FirstName = "Mike",
                LastName = "Hobo",
                Email = "emma@gmail.com",
                Hashed = "jk65ui",
                Salt = "ke978n",
                Plan = PaymentPlan.MONTHLY,
                UserType = UserType.GENERAL,
                CustomerID = "77777g",
                Students = new List<Student>()
            };
            Student student = new Student()
            {
                StudentID = -1,
                FirstName = "Man",
                LastName = "Hobo",
                Grade = 8
            };
            user.Students.Add(student);

            setUserInfoAccessor.mockUserTable.Add(user);

            setUserInfoEngine.InsertStudentInfo(user.UserID, user.Students);

            Assert.IsTrue(setUserInfoAccessor.mockStudentTable.Contains(student));
            Assert.AreEqual(student, setUserInfoAccessor.mockStudentTable.Where(s => s.StudentID == student.StudentID).ToList().ElementAt(0));
        }

        [TestMethod]
        public void TestUpdateStudentInfo()
        {
            User user = new User()
            {
                UserID = 40,
                FirstName = "Justin",
                LastName = "Bradley",
                Email = "jbradley@gmail.com",
                Hashed = "jdksi39dje93",
                Salt = "ke978n",
                Plan = PaymentPlan.MONTHLY,
                UserType = UserType.GENERAL,
                CustomerID = "129dde",
                Students = new List<Student>()
            };
            Student student = new Student()
            {
                StudentID = 40,
                FirstName = "Kid",
                LastName = "Bradley",
                Grade = 4
            };
            user.Students.Add(student);

            setUserInfoAccessor.mockUserTable.Add(user);
            setUserInfoAccessor.mockStudentTable.Add(student);

            student.FirstName = "Kent";
            student.Grade = 9;

            setUserInfoEngine.UpdateStudentInfo(user.Students);

            Assert.IsTrue(setUserInfoAccessor.mockStudentTable.Contains(student));
            Assert.AreEqual(student, setUserInfoAccessor.mockStudentTable.Where(s => s.StudentID == student.StudentID).ToList().ElementAt(0));
        }

        [TestMethod]
        public void TestDeleteStudentInfo()
        {
            User user = new User()
            {
                UserID = 50,
                FirstName = "Joe",
                LastName = "Homeboy",
                Email = "homeboy@gmail.com",
                Hashed = "jei3905us",
                Salt = "j723ie",
                Plan = PaymentPlan.MONTHLY,
                UserType = UserType.GENERAL,
                CustomerID = "5ui1a2",
                Students = new List<Student>()
            };
            Student student = new Student()
            {
                StudentID = 50,
                FirstName = "Dude",
                LastName = "Homeboy",
                Grade = 10
            };
            user.Students.Add(student);

            setUserInfoAccessor.mockUserTable.Add(user);
            setUserInfoAccessor.mockStudentTable.Add(student);

            setUserInfoEngine.DeleteStudentInfo(user.Students.Select(s => s.StudentID).ToList());

            Assert.IsFalse(setUserInfoAccessor.mockStudentTable.Contains(student));
        }
    }
}
