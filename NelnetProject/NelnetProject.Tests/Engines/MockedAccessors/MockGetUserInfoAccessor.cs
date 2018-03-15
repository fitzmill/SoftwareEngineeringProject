using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTOs;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockGetUserInfoAccessor : IGetUserInfoAccessor
    {
        public List<Student> StudentsDB;
        public List<User> MockDB;
        public MockGetUserInfoAccessor(List<Student> StudentsDB, List<User> MockDB)
        {
            this.StudentsDB = StudentsDB;
            this.MockDB = MockDB;
        }
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
