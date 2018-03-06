﻿using Core;
using Core.DTOs;
using Core.Interfaces;
using System.Data.SqlClient;

namespace Accessors
{
    public class GetUserInfoAccessor : IGetUserInfoAccessor
    {
        string connectionString;
        public GetUserInfoAccessor(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Gets a user's info from the database by a User's ID
        public User GetUserInfoByID(int userID)
        {
            string query = "[dbo].[GetUserInfoByUserID]";
            User result = new User();
            Student student = new Student();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@UserID", userID));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                var reader = command.ExecuteReader();
                bool firstRow = true;
                if (reader.Read())
                {
                    if (firstRow)
                    {
                        result.UserID = userID;
                        result.FirstName = reader.GetString(0);
                        result.LastName = reader.GetString(1);
                        result.Email = reader.GetString(2);
                        result.Hashed = reader.GetString(3);
                        result.Salt = reader.GetString(4);
                        result.PaymentPlan = (PaymentPlan)reader.GetByte(5);
                        result.UserType = (UserType)reader.GetByte(6);
                        result.CustomerID = reader.GetString(7);
                        firstRow = false;
                    }
                    student.StudentID = reader.GetInt32(8);
                    student.FirstName = reader.GetString(9);
                    student.LastName = reader.GetString(10);
                    student.Grade = reader.GetInt32(11);
                    result.Students.Add(student);
                }
            }
            return result;
        }

        // Gets a user's info from the database by a user's email
        public User GetUserInfoByEmail(string email)
        {
            string query = "[dbo].[GetUserInfoByEmail]";
            User result = new User();
            Student student = new Student();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@Email", email));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                var reader = command.ExecuteReader();
                bool firstRow = true;
                if (reader.Read())
                {
                    if (firstRow)
                    {
                        result.UserID = reader.GetInt32(0);
                        result.FirstName = reader.GetString(1);
                        result.LastName = reader.GetString(2);
                        result.Email = email;
                        result.Hashed = reader.GetString(3);
                        result.Salt = reader.GetString(4);
                        result.PaymentPlan = (PaymentPlan)reader.GetByte(5);
                        result.UserType = (UserType)reader.GetByte(6);
                        result.CustomerID = reader.GetString(7);
                        firstRow = false;
                    }
                    student.StudentID = reader.GetInt32(8);
                    student.FirstName = reader.GetString(9);
                    student.LastName = reader.GetString(10);
                    student.Grade = reader.GetInt32(11);
                    result.Students.Add(student);
                }
            }
            return result;
        }

        // Gets a user's password information for logging in
        public PasswordDTO GetUserPasswordInfo(string email)
        {
            string query = "[dbo].[GetPasswordInfo]";
            PasswordDTO result = new PasswordDTO();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@Email", email));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
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
            string query = "[dbo].[EmailExists]";
            bool result = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@Email", email));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = true;
                }
            }
            return result;
        }

        // Gets a user's Payment Spring customerID
        public string GetPaymentSpringCustomerID(int userID)
        {
            string query = "[dbo].[GetCustomerID]";
            string result = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@UserID", userID));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = reader.GetString(0);
                }
            }
            return result;
        }
    }
}
