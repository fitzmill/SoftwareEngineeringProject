using Core;
using Core.DTOs;
using Core.Interfaces;
using System.Configuration;
using System.Data.SqlClient;

namespace Accessors
{
    public class GetUserInfoAccessor : IGetUserInfoAccessor
    {
        string connectionString;
        public GetUserInfoAccessor()
        {
            connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;
        }

        // Gets a user's info from the database by a User's ID
        public User GetUserInfoByID(int userID)
        {
            string query = "[dbo].[GetUserInfoByUserID] @UserID=@passedInUserID";
            User result = new User();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@passedInUserID", userID);
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.UserID = userID;
                    result.FirstName = reader.GetString(0);
                    result.LastName = reader.GetString(1);
                    result.Email = reader.GetString(2);
                    result.Hashed = reader.GetString(3);
                    result.Salt = reader.GetString(4);
                    result.PaymentPlan = (PaymentPlan)reader.GetByte(6);
                    result.UserType = (UserType)reader.GetByte(7);
                    result.CustomerID = reader.GetInt32(8);
                }
            }
            return result;
        }

        // Gets a user's info from the database by a user's email
        public User GetUserInfoByEmail(string email)
        {
            string query = "[dbo].[GetUserInfoByEmail] @Email=@passedInEmail";
            User result = new User();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@passedInEmail", email);
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.UserID = reader.GetInt32(0);
                    result.FirstName = reader.GetString(1);
                    result.LastName = reader.GetString(2);
                    result.Email = email;
                    result.Hashed = reader.GetString(3);
                    result.Salt = reader.GetString(4);
                    result.PaymentPlan = (PaymentPlan)reader.GetByte(6);
                    result.UserType = (UserType)reader.GetByte(7);
                    result.CustomerID = reader.GetInt32(8);
                }
            }
            return result;
        }

        // Gets a user's password information for logging in
        public PasswordDTO GetUserPasswordInfo(string email)
        {
            string query = "[dbo].[GetPasswordInfo] @Email=@passedInEmail";
            PasswordDTO result = new PasswordDTO();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@passedInEmail", email);
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Hashed = reader.GetString(0);
                    result.Salt = reader.GetString(1);
                }
            }
            return result;
        }

        // Checks if an email already exists in the database.
        public bool EmailExists(string email)
        {
            string query = "[dbo].[EmailExists] @Email=@passedInEmail";
            bool result = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@passedInEmail", email);
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(0) != null)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        // Gets a user's Payment Spring customerID
        public string GetPaymentSpringCustomerID(int userID)
        {
            string query = "[dbo].[EmailExists] @userID=@passedInUserID";
            string result = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@passedInUserID", userID);
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString(0);
                }
            }
            return result;
        }
    }
}
