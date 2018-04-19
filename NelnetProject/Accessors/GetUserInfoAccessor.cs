using Core;
using Core.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Accessors
{
    public class GetUserInfoAccessor : IGetUserInfoAccessor
    {
        private readonly string _connectionString;

        public GetUserInfoAccessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<User> GetAllActiveUsers()
        {
            string query = "[dbo].[GetAllUsers]";
            IList<User> result = new List<User>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                conn.Open();
                var reader = command.ExecuteReader();
                bool firstRow = true;
                int? oldId = null;
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != oldId && oldId != null)
                    {
                        firstRow = true;
                    }
                    if (firstRow)
                    {
                        oldId = reader.GetInt32(0);
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
                            CustomerID = reader.GetString(8),
                            Students = new List<Student>(),
                        });
                        
                        firstRow = false;
                    }
                    
                    result[result.Count - 1].Students.Add(new Student()
                    {
                        StudentID = reader.GetInt32(9),
                        FirstName = reader.GetString(10),
                        LastName = reader.GetString(11),
                        Grade = reader.GetByte(12),
                    });
                }
            }
            return result;
        }

        public User GetUserInfoByID(int userID)
        {
            string query = "[dbo].[GetUserInfoByUserID]";
            User result = new User();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@UserID", userID));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                var reader = command.ExecuteReader();
                bool firstRow = true;
                while (reader.Read())
                {
                    if (firstRow)
                    {
                        result.UserID = userID;
                        result.FirstName = reader.GetString(0);
                        result.LastName = reader.GetString(1);
                        result.Email = reader.GetString(2);
                        result.Hashed = reader.GetString(3);
                        result.Salt = reader.GetString(4);
                        result.Plan = (PaymentPlan)reader.GetByte(5);
                        result.UserType = (UserType)reader.GetByte(6);
                        result.CustomerID = reader.GetString(7);
                        result.Students = new List<Student>();
                        firstRow = false;
                    }
                    result.Students.Add(new Student()
                    {
                        StudentID = reader.GetInt32(8),
                        FirstName = reader.GetString(9),
                        LastName = reader.GetString(10),
                        Grade = reader.GetByte(11)
                    });
                }
            }
            return result;
        }

        public User GetUserInfoByEmail(string email)
        {
            string query = "[dbo].[GetUserInfoByEmail]";
            User result = new User();
            Student student = new Student();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@Email", email));
                command.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                var reader = command.ExecuteReader();
                bool firstRow = true;
                while (reader.Read())
                {
                    if (firstRow)
                    {
                        result.UserID = reader.GetInt32(0);
                        result.FirstName = reader.GetString(1);
                        result.LastName = reader.GetString(2);
                        result.Email = email;
                        result.Hashed = reader.GetString(3);
                        result.Salt = reader.GetString(4);
                        result.Plan = (PaymentPlan)reader.GetByte(5);
                        result.UserType = (UserType)reader.GetByte(6);
                        result.CustomerID = reader.GetString(7);
                        result.Students = new List<Student>();
                        firstRow = false;
                    }
                    result.Students.Add(new Student()
                    {
                        StudentID = reader.GetInt32(8),
                        FirstName = reader.GetString(9),
                        LastName = reader.GetString(10),
                        Grade = reader.GetByte(11)
                    });
                }
            }
            return result;
        }

        public bool EmailExists(string email)
        {
            string query = "[dbo].[EmailExists]";
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
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

        public string GetPaymentSpringCustomerID(int userID)
        {
            string query = "[dbo].[GetCustomerID]";
            string result = "";
            using (SqlConnection conn = new SqlConnection(_connectionString))
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
