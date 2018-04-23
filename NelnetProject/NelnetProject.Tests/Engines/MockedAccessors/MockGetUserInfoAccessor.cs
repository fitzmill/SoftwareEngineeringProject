using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTOs;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockGetUserInfoAccessor : IGetUserInfoAccessor
    {
        public List<Student> _studentsDB;
        public List<User> _userDB;
        public MockGetUserInfoAccessor(List<Student> studentsDB, List<User> userDB)
        {
            _studentsDB = studentsDB;
            _userDB = userDB;
        }

        public bool EmailExists(string email)
        {
            return _userDB.Select(x => x.Email).Contains(email);
        }

        public IList<User> GetAllActiveUsers()
        {
            return _userDB;
        }

        public User GetUserInfoByID(int userID)
        {
            return _userDB.FirstOrDefault(x => x.UserID == userID);
        }

        public User GetUserInfoByEmail(string email)
        {
            return _userDB.FirstOrDefault(x => x.Email == email);
        }

        public string GetPaymentSpringCustomerID(int userID)
        {
            return _userDB.FirstOrDefault(u => u.UserID == userID)?.CustomerID;
        }
    }
}
