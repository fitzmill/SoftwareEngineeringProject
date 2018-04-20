using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Accessors
{
    /// <summary>
    /// Helper for making connections to the SQL database more convenient.
    /// </summary>
    public class SqlConnectionFactory
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;

        /// <summary>
        /// Creates and opens a new connection to the internal connection string.
        /// </summary>
        /// <returns>A new, opened connection</returns>
        public static SqlConnection CreateConnection()
        {
            SqlConnection con = new SqlConnection(_connectionString);
            con.Open();
            return con;
        }

        /// <summary>
        /// Executes an SQL query by creating a connection, command, and reader, then disposes of them.
        /// </summary>
        /// <param name="query">the SQL query string</param>
        /// <param name="parms">the parameters to be inserted into the query</param>
        /// <param name="readFunc">the action to be taken on the reader</param>
        public static void RunSqlQuery(string query, Dictionary<string, object> parms, Action<SqlDataReader> readFunc)
        {
            using (SqlConnection con = CreateConnection())
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (parms != null)
                    {
                        foreach (KeyValuePair<string, object> entry in parms)
                        {
                            command.Parameters.Add(new SqlParameter(entry.Key, entry.Value));
                        }
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        readFunc(reader);
                    }
                }
            }
        }

        /// <summary>
        /// Executes an SQL query by creating a connection, command, and reader, then disposes of them.
        /// </summary>
        /// <param name="query">the SQL query string</param>
        /// <param name="readFunc">the action to be taken on the reader</param>
        public static void RunSqlQuery(string query, Action<SqlDataReader> readFunc)
        {
            RunSqlQuery(query, null, readFunc);
        }

        /// <summary>
        /// Executes a SQL query that does not return any data.
        /// </summary>
        /// <param name="query">the SQL query string</param>
        /// <param name="parms">the parameters to be inserted into the query; defaults to null</param>
        /// <returns>the number of rows affected</returns>
        public static int RunSqlNonQuery(string query, Dictionary<string, object> parms = null)
        {
            using (SqlConnection con = CreateConnection())
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (parms != null)
                    {
                        foreach (KeyValuePair<string, object> entry in parms)
                        {
                            command.Parameters.Add(new SqlParameter(entry.Key, entry.Value));
                        }
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
