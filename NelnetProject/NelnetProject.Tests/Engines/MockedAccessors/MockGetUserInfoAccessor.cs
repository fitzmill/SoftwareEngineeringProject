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
            var user = UserDB.FirstOrDefault(x => x.Email == email);
            return new PasswordDTO()
            {
                Hashed = user.Hashed,
                Salt = user.Salt,
                UserType = user.UserType
            };
        }

        public string GetPaymentSpringCustomerID(int userID)
        {
            return UserDB.FirstOrDefault(u => u.UserID == userID)?.CustomerID;
        }
    }
}
