using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accessors
{
    public class SqlConnectionFactory
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;

        public static SqlConnection CreateConnection()
        {
            SqlConnection con = new SqlConnection(_connectionString);
            con.Open();
            return con;
        }

        public static void RunSqlQuery(string query, Action<SqlDataReader> readFunc)
        {
            using (SqlConnection con = CreateConnection())
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        readFunc(reader);
                    }
                }
            }
        }
    }
}
