using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Interfaces;
using Core.DTOs;

namespace Engines
{
    public class GetUserInfoEngine : IGetUserInfoEngine
    {
        // Checks to see if a given email matches with an email in the database.
        public bool EmailExists(string email)
        {
            throw new NotImplementedException();
        }

        // Validates the login of a user given an email and a password.
        public bool ValidateLoginInfo(string email, string password)
        {
            throw new NotImplementedException();
        }

        // Gets a user's information when given a user's ID.
        public User GetUserInfoByID(int userID)
        {
            throw new NotImplementedException();
        }

        // Gets a user's information when given an email.
        public User GetUserInfoByEmail(string email)
        {
            throw new NotImplementedException();
        }

        // Gets a user's payment info from Payment Spring with their customer ID.
        public UserPaymentInfoDTO GetPaymentInfoForUser(string customerID)
        {
            throw new NotImplementedException();
        }
    }
}
