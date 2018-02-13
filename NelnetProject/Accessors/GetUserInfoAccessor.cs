using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.DTOs;
using Core.Interfaces;

namespace Accessors
{
    class GetUserInfoAccessor : IGetUserInfoAccessor
    {
        // Gets a user's info from the database by a User's ID
        public User GetUserInfoByID(int userID)
        {
            throw new NotImplementedException();
        }

        // Gets a user's info from the database by a user's email
        public User GetUserInfoByEmail(string email)
        {
            throw new NotImplementedException();
        }

        // Gets a user's password information for logging in
        public PasswordDTO GetUserPasswordInfo(string email)
        {
            throw new NotImplementedException();
        }

        // Checks if an email already exists in the database.
        public bool EmailExists(string email)
        {
            throw new NotImplementedException();
        }

        // Gets a user's Payment Spring customerID
        public string GetPaymentSpringCustomerID(int userID)
        {
            throw new NotImplementedException();
        }
    }
}
