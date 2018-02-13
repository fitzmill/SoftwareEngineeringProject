using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Core;
using System.Configuration;
using System.Data.SqlClient;

namespace Accessors
{
    public class GetTransactionAccessor : IGetTransactionAccessor
    {
        string connectionString;
        public GetTransactionAccessor()
        {
            connectionString = ConfigurationManager.ConnectionStrings["NelnetPaymentProcessing"].ConnectionString;
        }

        //Executes a stored procedure in the database for getting all Transactions with userID as a parameter
        public List<Transaction> GetAllTransactionsForUser(int userID)
        {
            string query = "[dbo].[GetAllTransactionsForUser] @UserID=@passedInUserID";

            var result = new List<Transaction>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@passedInUserID", userID);

                conn.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new Transaction()
                    {
                        TransactionID = reader.GetInt32(0),
                        UserID = reader.GetInt32(1),
                        AmountCharged = reader.GetDouble(2),
                        DateDue = reader.GetDateTime(3),
                        DateCharged = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                        ProcessState = (ProcessState)reader.GetByte(5),
                        ReasonFailed = (ReasonFailed?)(reader.IsDBNull(6) ? (int?)null : reader.GetByte(6))
                    });
                }
            }
            return result;
        }

        //Executes a stored procedure in the database for getting all Transactions with ProcessState as a parameter
        public List<Transaction> GetAllUnsettledTransactions()
        {
            throw new NotImplementedException();
        }

        //Executes a stored procedure in the database for getting the most recent transaction with userID as a parameter
        public Transaction GetMostRecentTransactionForUser(int userID)
        {
            throw new NotImplementedException();
        }

        //Executes a stored procedure in the database for getting all Transactions with startTime and endTime as parameters
        public List<Transaction> GetTransactionsForDateRange(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
