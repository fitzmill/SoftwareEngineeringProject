using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTOs;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockGetUserInfoAccessor : IGetUserInfoAccessor
    {
        public List<Student> studentsDB;
        public List<User> userDB;
        public MockGetUserInfoAccessor(List<Student> studentsDB, List<User> userDB)
        {
            this.studentsDB = studentsDB;
            this.userDB = userDB;
        }

        public bool EmailExists(string email)
        {
            return userDB.Select(x => x.Email).Contains(email);
        }

        public IList<User> GetAllActiveUsers()
        {
            return userDB;
        }

        public User GetUserInfoByID(int userID)
        {
            return userDB.FirstOrDefault(x => x.UserID == userID);
        }

        public User GetUserInfoByEmail(string email)
        {
            return userDB.FirstOrDefault(x => x.Email == email);
        }

        public string GetPaymentSpringCustomerID(int userID)
        {
            return userDB.FirstOrDefault(u => u.UserID == userID)?.CustomerID;
        }
    }
}
