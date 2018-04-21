using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        /// <param name="parameters">the parameters to be inserted into the query</param>
        /// <param name="type">the type of query to execute</param>
        /// <param name="readFunc">the action to be taken on the reader</param>
        public static void RunSqlQuery(string query, Dictionary<string, object> parameters, CommandType type, Action<SqlDataReader> readFunc)
        {
            using (SqlConnection con = CreateConnection())
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> entry in parameters)
                        {
                            command.Parameters.Add(new SqlParameter(entry.Key, entry.Value));
                        }
                    }

                    command.CommandType = type;

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
        /// <param name="parameters">the parameters to be inserted into the query</param>
        /// <param name="readFunc">the action to be taken on the reader</param>
        public static void RunSqlQuery(string query, Dictionary<string, object> parameters, Action<SqlDataReader> readFunc)
        {
            RunSqlQuery(query, parameters, CommandType.Text, readFunc);
        }

        /// <summary>
        /// Executes an SQL query by creating a connection, command, and reader, then disposes of them.
        /// </summary>
        /// <param name="query">the SQL query string</param>
        /// <param name="type">the type of SQL query to execute</param>
        /// <param name="readFunc">the action to be taken on the reader</param>
        public static void RunSqlQuery(string query, CommandType type, Action<SqlDataReader> readFunc)
        {
            RunSqlQuery(query, null, type,  readFunc);
        }

        /// <summary>
        /// Executes an SQL query by creating a connection, command, and reader, then disposes of them.
        /// </summary>
        /// <param name="query">the SQL query string</param>
        /// <param name="readFunc">the action to be taken on the reader</param>
        public static void RunSqlQuery(string query, Action<SqlDataReader> readFunc)
        {
            RunSqlQuery(query, null, CommandType.Text, readFunc);
        }

        /// <summary>
        /// Executes a SQL query that does not return any data.
        /// </summary>
        /// <param name="query">the SQL query string</param>
        /// <param name="parameters">the parameters to be inserted into the query; defaults to null</param>
        /// <returns>the number of rows affected</returns>
        public static int RunSqlNonQuery(string query, Dictionary<string, object> parameters = null, CommandType type = CommandType.Text, SqlParameter returnParameter = null)
        {
            using (SqlConnection con = CreateConnection())
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (KeyValuePair<string, object> entry in parameters)
                        {
                            command.Parameters.Add(new SqlParameter(entry.Key, entry.Value));
                        }
                    }

                    command.CommandType = type;

                    if (returnParameter != null)
                    {
                        command.Parameters.Add(returnParameter);
                        returnParameter.Direction = ParameterDirection.Output;
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
