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
            string query = "[dbo].[InsertPersonalInfo]";

            //add the personal record from the user to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                //fill in parameters
                command.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
                command.Parameters.Add(new SqlParameter("@LastName", user.LastName));
                command.Parameters.Add(new SqlParameter("@Email", user.Email));
                command.Parameters.Add(new SqlParameter("@Hashed", user.Hashed));
                command.Parameters.Add(new SqlParameter("@Salt", user.Salt));
                command.Parameters.Add(new SqlParameter("@PaymentPlan", user.PaymentPlan));
                command.Parameters.Add(new SqlParameter("@UserType", user.UserType));
                command.Parameters.Add(new SqlParameter("@CustomerID", user.CustomerID));

                //set the command type
                command.CommandType = CommandType.StoredProcedure;

                //open connection
                connection.Open();

                //execute command
                var reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    //read the id of the user that has just been created
                    user.UserID = (int)reader.GetDecimal(0);
                }
            }
        }

        //use the id from the user model to update the record asociated with the user
        public void UpdatePersonalInfo(User user)
        {
            string query = "[dbo].[UpdatePersonalInfo]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                //fill in parameters
                command.Parameters.Add(new SqlParameter("@ID", user.UserID));
                command.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
                command.Parameters.Add(new SqlParameter("@LastName", user.LastName));
                command.Parameters.Add(new SqlParameter("@Email", user.Email));
                command.Parameters.Add(new SqlParameter("@Hashed", user.Hashed));
                command.Parameters.Add(new SqlParameter("@Salt", user.Salt));
                command.Parameters.Add(new SqlParameter("@PaymentPlan", user.PaymentPlan));
                command.Parameters.Add(new SqlParameter("@UserType", user.UserType));
                command.Parameters.Add(new SqlParameter("@CustomerID", user.CustomerID));

                //set command type
                command.CommandType = CommandType.StoredProcedure;

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
            string query = "[dbo].[DeletePersonalInfo]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@ID", userID));

                command.CommandType = CommandType.StoredProcedure;

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
            string query = "[dbo].[InsertStudentInfo]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@FirstName", student.FirstName));
                command.Parameters.Add(new SqlParameter("@LastName", student.LastName));
                command.Parameters.Add(new SqlParameter("@Grade", student.Grade));
                command.Parameters.Add(new SqlParameter("@UserID", userID));

                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    student.StudentID = (int)reader.GetDecimal(0);
                }
            }
        }

        //update a student in the database with the information provided
        public void UpdateStudentInfo(Student student)
        {
            string query = "[dbo].[UpdateStudentInfo]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@ID", student.StudentID));
                command.Parameters.Add(new SqlParameter("@FirstName", student.FirstName));
                command.Parameters.Add(new SqlParameter("@LastName", student.LastName));
                command.Parameters.Add(new SqlParameter("@Grade", student.Grade));

                command.CommandType = CommandType.StoredProcedure;

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
        public void DeleteStudentInfoByStudentID(int studentID)
        {
            string query = "[dbo].[DeleteStudentInfoByStudentID]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@ID", studentID));

                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    //TODO: define custom exception type for this error
                    throw new Exception("Database query executed incorrectly");
                }
            }
        }

        //delete all the students associated with a specific user id
        public void DeleteStudentInfoByUserID(int userID)
        {
            string query = "[dbo].[DeleteStudentInforByUserID]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@ID", userID));

                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}
