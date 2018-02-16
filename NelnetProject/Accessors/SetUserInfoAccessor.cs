using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Accessors
{
    public class SetUserInfoAccessor : ISetUserInfoAccessor
    {
        string connectionString;

        public SetUserInfoAccessor(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //insert a new record into the user table
        public void InsertPersonalInfo(User user)
        { 
            //specifies stored query and parameters
            string query = "[dbo].[InsertPersonalInfo] @FirstName=_FirstName, @LastName=_LastName, @Email=_Email, @Hashed=_Hashed, @Salt=_Salt, @PaymentPlan=_PaymentPlan, @UserType=_UserType, @CustomerID=_CustomerID";

            //add the personal record from the user to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                //fill in parameters
                command.Parameters.AddWithValue("_FirstName", user.FirstName);
                command.Parameters.AddWithValue("_LastName", user.LastName);
                command.Parameters.AddWithValue("_Email", user.Email);
                command.Parameters.AddWithValue("_Hashed", user.Hashed);
                command.Parameters.AddWithValue("_Salt", user.Salt);
                command.Parameters.AddWithValue("_PaymentPlan", user.PaymentPlan);
                command.Parameters.AddWithValue("_UserType", user.UserType);
                command.Parameters.AddWithValue("_CustomerID", user.CustomerID);

                //open connection
                connection.Open();

                //execute command
                var reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    //read the id of the user that has just been created
                    user.UserID = reader.GetInt32(0);
                }
            }
        }

        //use the id from the user model to update the record asociated with the user
        public void UpdatePersonalInfo(User user)
        {
            string query = "[dbo].[UpdatePersonalInfo] @ID=_ID, @FirstName=_FirstName, @LastName=_LastName, @Email=_Email, @Hashed=_Hashed, @Salt=_Salt, @PaymentPlan=_PaymentPlan, @UserType=_UserType, @CustomerID=_CustomerID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                //fill in parameters
                command.Parameters.AddWithValue("_ID", user.UserID);
                command.Parameters.AddWithValue("_FirstName", user.FirstName);
                command.Parameters.AddWithValue("_LastName", user.LastName);
                command.Parameters.AddWithValue("_Email", user.Email);
                command.Parameters.AddWithValue("_Hashed", user.Hashed);
                command.Parameters.AddWithValue("_Salt", user.Salt);
                command.Parameters.AddWithValue("_PaymentPlan", user.PaymentPlan);
                command.Parameters.AddWithValue("_UserType", user.UserType);
                command.Parameters.AddWithValue("_CustomerID", user.CustomerID);

                //open connection
                connection.Open();

                //execute command
                int rowsAffected = command.ExecuteNonQuery();
                
                if(rowsAffected != 1)
                {
                    //TODO: define custom exception type for this error
                    throw new Exception("Database query executed incorrectly");
                }
            }
        }

        //remove all personal data from the database associated with the userID
        public void DeletePersonalInfo(int userID)
        {
            string query = "[dbo].[DeletePersonalInfo] @ID=_ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("_ID", userID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    //TODO: define custom exception type for this error
                    throw new Exception("Database query executed incorrectly");
                }
            }
        }

        //add a new student to the database associated with the userID
        public void InsertStudentInfo(int userID, Student student)
        {
            string query = "[dbo].[InsertStudentInfo] @FirstName=_FirstName, @LastName=_LastName, @Grade=_Grade, @UserID=_UserID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("_FirstName", student.FirstName);
                command.Parameters.AddWithValue("_LastName", student.LastName);
                command.Parameters.AddWithValue("_Grade", student.Grade);
                command.Parameters.AddWithValue("_UserID", userID);

                connection.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    student.StudentID = reader.GetInt32(0);
                }
            }
        }

        //update a student in the database with the information provided
        public void UpdateStudentInfo(Student student)
        {
            string query = "[dbo].[UpdateStudentInfo] @ID=_ID, @FirstName=_FirstName, @LastName=_LastName, @Grade=_Grade";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("_ID", student.StudentID);
                command.Parameters.AddWithValue("_FirstName", student.FirstName);
                command.Parameters.AddWithValue("_LastName", student.LastName);
                command.Parameters.AddWithValue("_Grade", student.Grade);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    //TODO: define custom exception type for this error
                    throw new Exception("Database query executed incorrectly");
                }
            }
        }

        //remove data associated with the particular studentID
        public void DeleteStudentInfo(int studentID)
        {
            string query = "[dbo].[DeleteStudentInfo] @ID=_ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("_ID", studentID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    //TODO: define custom exception type for this error
                    throw new Exception("Database query executed incorrectly");
                }
            }
        }
    }
}
