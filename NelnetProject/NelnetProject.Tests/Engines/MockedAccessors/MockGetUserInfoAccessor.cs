﻿using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core;
using Core.DTOs;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockGetUserInfoAccessor : IGetUserInfoAccessor
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
        public List<User> MockDB = new List<User>()
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

        public MockGetUserInfoAccessor()
        {
            MockDB.ElementAt(0).Students.Add(StudentsDB.ElementAt(0));
            MockDB.ElementAt(0).Students.Add(StudentsDB.ElementAt(1));
            MockDB.ElementAt(1).Students.Add(StudentsDB.ElementAt(2));
        }

        public bool EmailExists(string email)
        {
            return MockDB.Select(x => x.Email).Contains(email);
        }

        public IList<User> GetAllUsers()
        {
            return MockDB;
        }

        public User GetUserInfoByID(int userID)
        {
            return MockDB.FirstOrDefault(x => x.UserID == userID);
        }

        public User GetUserInfoByEmail(string email)
        {
            return MockDB.FirstOrDefault(x => x.Email == email);
        }

        public PasswordDTO GetUserPasswordInfo(string email)
        {
            PasswordDTO result = new PasswordDTO();
            if (email.Equals(MockDB[0].Email))
            {
                result.Hashed = MockDB[0].Hashed;
                result.Salt = MockDB[0].Salt;
                return result;
            }
            return null;
        }

        public string GetPaymentSpringCustomerID(int userID)
        {
            if (userID == MockDB[0].UserID)
            {
                return MockDB[0].CustomerID;
            }
            return null;
        }
    }
}
