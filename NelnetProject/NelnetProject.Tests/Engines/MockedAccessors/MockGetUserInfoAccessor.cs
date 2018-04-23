﻿using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTOs;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockGetUserInfoAccessor : IGetUserInfoAccessor
    {
        public List<Student> StudentsDB;
        public List<User> UserDB;
        public MockGetUserInfoAccessor(List<Student> StudentsDB, List<User> UserDB)
        {
            this.StudentsDB = StudentsDB;
            this.UserDB = UserDB;
        }

        public bool EmailExists(string email)
        {
            return UserDB.Select(x => x.Email).Contains(email);
        }

        public IList<User> GetAllActiveUsers()
        {
            return UserDB;
        }

        public User GetUserInfoByID(int userID)
        {
            return UserDB.FirstOrDefault(x => x.UserID == userID);
        }

        public User GetUserInfoByEmail(string email)
        {
            return UserDB.FirstOrDefault(x => x.Email == email);
        }

        public PasswordDTO GetUserPasswordInfo(string email)
        {
            PasswordDTO result = new PasswordDTO();
            if (email.Equals(UserDB[0].Email))
            {
                result.Hashed = UserDB[0].Hashed;
                result.Salt = UserDB[0].Salt;
                return result;
            }
            return null;
        }

        public string GetPaymentSpringCustomerID(int userID)
        {
            return UserDB.FirstOrDefault(u => u.UserID == userID)?.CustomerID;
        }
    }
}
