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
        public void DeleteStudentInfoByStudentID(int studentID)
        {
            string query = "DELETE FROM [dbo].[Student] WHERE StudentID = @ID";

            var parameters = new Dictionary<string, object>()
            {
                { "@ID", studentID }
            };

            var rowsAffected = SqlConnectionFactory.RunSqlNonQuery(query, parameters);

            if (rowsAffected != 1)
            {
                throw new SqlRowNotAffectedException("Student could not be found to be deleted");
            }
        }

        public void DeleteStudentInfoByUserID(int userID)
        {
            string query = "DELETE FROM [dbo].[Student] WHERE UserID = @ID";

            var parameters = new Dictionary<string, object>()
            {
                { "@ID", userID }
            };

            var rowsAffected = SqlConnectionFactory.RunSqlNonQuery(query, parameters);

            if (rowsAffected < 0)
            {
                throw new SqlRowNotAffectedException("Students could not be found to be deleted");
            }
        }

        public Student GetStudentInfoByID(int studentID)
        {
            string query = "SELECT * FROM [dbo].[Student] WHERE StudentID = @ID";

            var parameters = new Dictionary<string, object>()
            {
                { "@ID", studentID }
            };

            Student result = null;

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
                if (reader.Read())
                {
                    result = new Student()
                    {
                        StudentID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Grade = reader.GetByte(3),
                        UserID = reader.GetInt32(4),
                    };
                }
            });

            return result;
        }

        public IEnumerable<Student> GetStudentInfoByUserID(int userID)
        {
            string query = "SELECT * FROM [dbo].[Student] WHERE UserID = @ID";

            var parameters = new Dictionary<string, object>()
            {
                { "@ID", userID }
            };

            IList<Student> result = new List<Student>();

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
                while (reader.Read())
                {
                    result.Add(new Student()
                    {
                        StudentID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Grade = reader.GetByte(3),
                        UserID = reader.GetInt32(4),
                    });
                }
            });

            return result;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            string query = "SELECT * FROM [dbo].[Student]";

            IList<Student> result = new List<Student>();

            SqlConnectionFactory.RunSqlQuery(query, reader =>
            {
                while (reader.Read())
                {
                    result.Add(new Student()
                    {
                        StudentID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Grade = reader.GetInt32(3),
                        UserID = reader.GetInt32(4),
                    });
                }
            });

            return result;
        }

        public void InsertStudentInfo(int userID, Student student)
        {
            string query = "INSERT INTO [dbo].[Student] (FirstName, LastName, Grade, UserID) " +
                            "VALUES(@FirstName, @LastName, @Grade, @UserID)";

            var parameters = new Dictionary<string, object>()
            {
                { "@FirstName", student.FirstName },
                { "@LastName", student.LastName },
                { "@Grade", student.Grade },
                { "@UserID", userID }
            };

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
                if (reader.Read())
                {
                    student.StudentID = (int)reader.GetDecimal(0);
                }
            });
        }

        public void UpdateStudentInfo(Student student)
        {
            string query = "UPDATE [dbo].[Student] " +
                           "SET FirstName = @FirstName, LastName = @LastName, Grade = @Grade " +
                           "WHERE StudentID = @ID";

            var parameters = new Dictionary<string, object>()
            {
                { "@FirstName", student.FirstName },
                { "@LastName", student.LastName },
                { "@Grade", student.Grade },
                { "@ID", student.StudentID }
            };

            var rowsAffected = SqlConnectionFactory.RunSqlNonQuery(query, parameters);

            if (rowsAffected != 1)
            {
                throw new SqlRowNotAffectedException("Student could not be found to update");
            }
        }
    }
}
