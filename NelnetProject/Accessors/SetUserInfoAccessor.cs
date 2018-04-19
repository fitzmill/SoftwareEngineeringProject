using Core;
using Core.Exceptions;
using Core.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Accessors
{
    public class SetUserInfoAccessor : ISetUserInfoAccessor
    {
        private readonly string _connectionString;

        public SetUserInfoAccessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InsertPersonalInfo(User user)
        { 
            string query = "[dbo].[InsertPersonalInfo]";
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
                command.Parameters.Add(new SqlParameter("@LastName", user.LastName));
                command.Parameters.Add(new SqlParameter("@Email", user.Email));
                command.Parameters.Add(new SqlParameter("@Hashed", user.Hashed));
                command.Parameters.Add(new SqlParameter("@Salt", user.Salt));
                command.Parameters.Add(new SqlParameter("@PaymentPlan", user.Plan));
                command.Parameters.Add(new SqlParameter("@UserType", user.UserType));
                command.Parameters.Add(new SqlParameter("@CustomerID", user.CustomerID));

                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                var reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    user.UserID = (int)reader.GetDecimal(0);
                }
            }
        }

        public void UpdatePersonalInfo(User user)
        {
            string query = "[dbo].[UpdatePersonalInfo]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@ID", user.UserID));
                command.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
                command.Parameters.Add(new SqlParameter("@LastName", user.LastName));
                command.Parameters.Add(new SqlParameter("@Email", user.Email));
                command.Parameters.Add(new SqlParameter("@Hashed", user.Hashed));
                command.Parameters.Add(new SqlParameter("@Salt", user.Salt));
                command.Parameters.Add(new SqlParameter("@PaymentPlan", user.Plan));
                command.Parameters.Add(new SqlParameter("@UserType", user.UserType));
                command.Parameters.Add(new SqlParameter("@CustomerID", user.CustomerID));

                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                
                if(rowsAffected != 1)
                {
                    throw new SqlRowNotAffectedException("Could not find user record to update");
                }
            }
        }

        public void DeletePersonalInfo(int userID)
        {
            string query = "[dbo].[DeletePersonalInfo]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@ID", userID));

                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    throw new SqlRowNotAffectedException("User record could not be found to delete");
                }
            }
        }

        public void InsertStudentInfo(int userID, Student student)
        {
            string query = "[dbo].[InsertStudentInfo]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
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

        public void UpdateStudentInfo(Student student)
        {
            string query = "[dbo].[UpdateStudentInfo]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
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
                    throw new SqlRowNotAffectedException("Student could not be found to update");
                }
            }
        }

        public void DeleteStudentInfoByStudentID(int studentID)
        {
            string query = "[dbo].[DeleteStudentInfoByStudentID]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@ID", studentID));

                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    throw new SqlRowNotAffectedException("Student could not be found to be deleted");
                }
            }
        }

        public void DeleteStudentInfoByUserID(int userID)
        {
            string query = "[dbo].[DeleteStudentInfoByUserID]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@ID", userID));

                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected < 0)
                {
                    throw new SqlRowNotAffectedException("Students could not be found to be deleted");
                }
            }
        }
    }
}
