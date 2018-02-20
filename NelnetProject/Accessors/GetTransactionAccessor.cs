using Core.Interfaces;
using System;
using System.Collections.Generic;
using Core;
using System.Data.SqlClient;

namespace Accessors
{
    public class GetTransactionAccessor : IGetTransactionAccessor
    {
        string connectionString;
        public GetTransactionAccessor(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //Executes a stored procedure in the database for getting all Transactions with userID as a parameter
        public IList<Transaction> GetAllTransactionsForUser(int userID)
        {
            //this is the format for executing stored procedures in SQL
            string query = "[dbo].[GetAllTransactionsForUser] @UserID=@passedInUserID";

            //initialize return object
            var result = new List<Transaction>();

            //this makes the connection, but doesn't open it. The connection will automatically be disposed of at the end of the using braces
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //makes the SQL command with the query string and connection object
                SqlCommand command = new SqlCommand(query, conn);

                //replaces the @passedInUserID that's in the query string with the integer userID
                command.Parameters.AddWithValue("@passedInUserID", userID);

                conn.Open();

                //what actually executes the command
                var reader = command.ExecuteReader();

                //using a while loop since I'm expecting multiple rows from the query
                //each iteration will contain one row from the Transaction table
                while (reader.Read())
                {
                    //Adds the new transaction to the result set
                    result.Add(new Transaction()
                    {
                        TransactionID = reader.GetInt32(0),
                        UserID = reader.GetInt32(1),
                        AmountCharged = reader.GetDouble(2),
                        DateDue = reader.GetDateTime(3),
                        //one line if statements have to return the same type, so nullable simple types need to be explicitly casted
                        DateCharged = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                        ProcessState = (ProcessState)reader.GetByte(5),
                        ReasonFailed = (ReasonFailed?)(reader.IsDBNull(6) ? (int?)null : reader.GetByte(6))
                    });
                }
            }
            return result;
        }

        //Executes a stored procedure in the database for getting all Transactions with ProcessState as a parameter
        public IList<Transaction> GetAllUnsettledTransactions()
        {
            throw new NotImplementedException();
        }

        //Executes a stored procedure in the database for getting the most recent transaction with userID as a parameter
        public Transaction GetMostRecentTransactionForUser(int userID)
        {
            string query = "[dbo].[GetMostRecentTransactionForUser] @UserID = @passInUserID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@passInUserID", userID);

                conn.Open();
                var reader = command.ExecuteReader();

                //Only expecting one row to be returned
                if (reader.Read())
                {
                    return new Transaction()
                    {
                        TransactionID = reader.GetInt32(0),
                        UserID = reader.GetInt32(1),
                        AmountCharged = reader.GetDouble(2),
                        DateDue = reader.GetDateTime(3),
                        DateCharged = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                        ProcessState = (ProcessState)reader.GetByte(5),
                        ReasonFailed = (ReasonFailed?)(reader.IsDBNull(6) ? (int?)null : reader.GetByte(6))
                    };
                }      
            }

            return null;
        }

        //Executes a stored procedure in the database for getting all Transactions with startTime and endTime as parameters
        public IList<Transaction> GetTransactionsForDateRange(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
