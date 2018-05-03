using Core;
using Core.Interfaces.Accessors;
using System.Collections.Generic;
using System.Linq;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockUserAccessor : IUserAccessor
    {
        public List<User> MockDb;

        public MockUserAccessor(List<User> users)
        {
            MockDb = users;
        }
        public void DeletePersonalInfo(int userID)
        {
            MockDb.RemoveAll(x => x.UserID == userID);
        }

        public bool EmailExists(string email)
        {
            return MockDb.FirstOrDefault(x => x.Email == email) != null;
        }

        public IEnumerable<User> GetAllActiveUsers()
        {
            return MockDb.Where(x => x.UserType == UserType.GENERAL);
        }

        public string GetPaymentSpringCustomerID(int userID)
        {
            return MockDb.FirstOrDefault(x => x.UserID == userID)?.CustomerID;
        }

        public User GetUserInfoByEmail(string email)
        {
            return MockDb.FirstOrDefault(x => x.Email == email);
        }

        public User GetUserInfoByID(int userID)
        {
            return MockDb.FirstOrDefault(x => x.UserID == userID);
        }

        public void InsertPersonalInfo(User user)
        {
            MockDb.Add(user);
            user.UserID = MockDb.IndexOf(user) + 1;
        }

        public void UpdatePersonalInfo(User user)
        {
            var index = MockDb.IndexOf(user);
            MockDb.RemoveAll(x => x.UserID == user.UserID);
            if (index == MockDb.Count)
            {
                MockDb.Add(user);
            }
            else
            {
                MockDb.Insert(index, user);
            }
        }
    }
}
