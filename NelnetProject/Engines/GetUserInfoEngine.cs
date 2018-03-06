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
            return getUserInfoAccessor.GetAllUsers();
        }
        // Validates the login of a user given an email and a password.
        public bool ValidateLoginInfo(string email, string password)
        {
            PasswordDTO userPasswordInfo = getUserInfoAccessor.GetUserPasswordInfo(email);
            string hashedGivenPassword = PasswordUtils.HashPasswords(password, userPasswordInfo.Salt);
            if (hashedGivenPassword.Equals(userPasswordInfo.Hashed))
            {
                return true;
            } else
            {
                return false;
            }
        }

        // Gets a user's information when given a user's ID.
        public User GetUserInfoByID(int userID)
        {
            return getUserInfoAccessor.GetUserInfoByID(userID);
        }

        // Gets a user's information when given an email.
        public User GetUserInfoByEmail(string email)
        {
            return getUserInfoAccessor.GetUserInfoByEmail(email);
        }

        // Gets a user's payment info from Payment Spring with their customer ID.
        public UserPaymentInfoDTO GetPaymentInfoForUser(string customerID)
        {
            return getPaymentInfoAccessor.GetPaymentInfoForCustomer(customerID);
        }
    }
}
