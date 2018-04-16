using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Interfaces;
using Core.DTOs;
using Engines.Utils;

namespace Engines
{
    public class GetUserInfoEngine : IGetUserInfoEngine
    {
        IGetPaymentInfoAccessor getPaymentInfoAccessor;
        IGetUserInfoAccessor getUserInfoAccessor;

        public GetUserInfoEngine(IGetPaymentInfoAccessor getPaymentInfoAccessor, IGetUserInfoAccessor getUserInfoAccessor)
        {
            this.getPaymentInfoAccessor = getPaymentInfoAccessor;
            this.getUserInfoAccessor = getUserInfoAccessor;
        }
        // Checks to see if a given email matches with an email in the database.
        public bool EmailExists(string email)
        {
            return getUserInfoAccessor.EmailExists(email);
        }

        public IList<User> GetAllUsers()
        {
            return getUserInfoAccessor.GetAllActiveUsers();
        }
        // Validates the login of a user given an email and a password.
        public User ValidateLoginInfo(string email, string password)
        {
            var user = getUserInfoAccessor.GetUserInfoByEmail(email);
            string hashedGivenPassword = PasswordUtils.HashPasswords(password, user.Salt);
            if (hashedGivenPassword.Equals(user.Hashed))
            {
                return user;
            } else
            {
                return null;
            }
        }

        // Gets a user's information when given a user's ID.
        public User GetUserInfoByID(int userID)
        {
            return getUserInfoAccessor.GetUserInfoByID(userID);
        }

        // Gets a user's payment info from Payment Spring with their customer ID.
        public UserPaymentInfoDTO GetPaymentInfoForUser(int userID)
        {
            string customerID = getUserInfoAccessor.GetPaymentSpringCustomerID(userID);
            return getPaymentInfoAccessor.GetPaymentInfoForCustomer(customerID);
        }
    }
}
