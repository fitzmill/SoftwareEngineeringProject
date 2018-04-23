using Core;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Accessors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Accessors
{
    public class UserAccessor : IUserAccessor
    {
        public void DeletePersonalInfo(int userID)
        {
            string query = "UPDATE [dbo].[User] SET Active = 0 WHERE UserID = @ID";

            var parameters = new Dictionary<string, object>()
            {
                { "@ID", userID }
            };

            var rowsAffected = SqlConnectionFactory.RunSqlNonQuery(query, parameters);

            if (rowsAffected != 1)
            {
                throw new SqlRowNotAffectedException("User record could not be found to delete");
            }
        }

        public bool EmailExists(string email)
        {
            string query = "[dbo].[EmailExists]";
            bool result = false;

            var parameters = new Dictionary<string, object>()
            {
                { "@Email", email }
            };

            var outputParam = new SqlParameter("@Exists", SqlDbType.Bit);

            var test = SqlConnectionFactory.RunSqlNonQuery(query, parameters, CommandType.StoredProcedure, outputParam);
            result = Convert.ToBoolean(outputParam.Value);

            return result;
        }

        public IEnumerable<User> GetAllActiveUsers()
        {
            string query = String.Format("SELECT * FROM [dbo].[User] u WHERE u.UserType = {0:D} AND u.Active = 1", UserType.GENERAL);

            List<User> result = new List<User>();

            SqlConnectionFactory.RunSqlQuery(query, reader =>
            {
                while (reader.Read())
                {
                    result.Add(new User()
                    {
                        UserID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Hashed = reader.GetString(4),
                        Salt = reader.GetString(5),
                        Plan = (PaymentPlan)reader.GetByte(6),
                        UserType = (UserType)reader.GetByte(7),
                        CustomerID = reader.GetString(8)
                    });
                }
            });

            return result;
        }

        public string GetPaymentSpringCustomerID(int userID)
        {
            string query = "SELECT CustomerID FROM [dbo].[User] WHERE UserID = @UserID";
            string result = "";

            var parameters = new Dictionary<string, object>()
            {
                { "@UserID", userID }
            };

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
                if (reader.Read())
                {
                    result = reader.GetString(0);
                }
            });

            return result;
        }

        public User GetUserInfoByEmail(string email)
        {
            string query = "SELECT * FROM [dbo].[User] WHERE Email = @Email AND Active = 1";
            User result = null;

            var parameters = new Dictionary<string, object>()
            {
                { "@Email", email }
            };

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
                if (reader.Read())
                {
                    result = new User()
                    {
                        UserID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Hashed = reader.GetString(4),
                        Salt = reader.GetString(5),
                        Plan = (PaymentPlan)reader.GetByte(6),
                        UserType = (UserType)reader.GetByte(7),
                        CustomerID = reader.GetString(8)
                    };
                }
            });
           
            return result;
        }

        public User GetUserInfoByID(int userID)
        {
            string query = "SELECT * FROM [dbo].[User] WHERE UserID = @UserID AND Active = 1";
            User result = null;

            var parameters = new Dictionary<string, object>()
            {
                { "@UserID", userID }
            };

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
                if (reader.Read())
                {
                    result = new User()
                    {
                        UserID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        Hashed = reader.GetString(4),
                        Salt = reader.GetString(5),
                        Plan = (PaymentPlan)reader.GetByte(6),
                        UserType = (UserType)reader.GetByte(7),
                        CustomerID = reader.GetString(8)
                    };
                }
            });

            return result;
        }

        public void InsertPersonalInfo(User user)
        {
            string query = "INSERT INTO [dbo].[User] (FirstName, LastName, Email, Hashed, Salt, PaymentPlan, UserType, CustomerID) " +
                           "VALUES(@FirstName, @LastName, @Email, @Hashed, @Salt, @PaymentPlan, @UserType, @CustomerID); SELECT SCOPE_IDENTITY()";

            var parameters = new Dictionary<string, object>()
            {
                { "@FirstName", user.FirstName },
                { "@LastName", user.LastName },
                { "@Email", user.Email },
                { "@Hashed", user.Hashed },
                { "@Salt", user.Salt },
                { "@PaymentPlan", user.Plan },
                { "@UserType", user.UserType },
                { "@CustomerID", user.CustomerID }
            };

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
                if (reader.Read())
                {
                    user.UserID = (int)reader.GetDecimal(0);
                }
            });
        }

        public void UpdatePersonalInfo(User user)
        {
            string query = "UPDATE [dbo].[User] SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Hashed = @Hashed, " +
                "Salt = @Salt, PaymentPlan = @PaymentPlan, UserType = @UserType, CustomerID = @CustomerID WHERE UserID = @ID";

            var parameters = new Dictionary<string, object>()
            {
                { "@FirstName", user.FirstName },
                { "@LastName", user.LastName },
                { "@Email", user.Email },
                { "@Hashed", user.Hashed },
                { "@Salt", user.Salt },
                { "@PaymentPlan", user.Plan },
                { "@UserType", user.UserType },
                { "@CustomerID", user.CustomerID },
                { "@ID", user.UserID }
            };

            var rowsAffected = SqlConnectionFactory.RunSqlNonQuery(query, parameters);

            if (rowsAffected != 1)
            {
                throw new SqlRowNotAffectedException("Could not find user record to update");
            }
        }
    }
}
