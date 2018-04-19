using Core;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Accessors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accessors
{
    public class StudentAccessor : IStudentAccessor
    {
        private readonly string _connectionString;

        public StudentAccessor(string connectionString)
        {
            _connectionString = connectionString;
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

        public Student GetStudentInfoByID(int studentID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudentInfoByUserID(int userID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            throw new NotImplementedException();
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
    }
}
