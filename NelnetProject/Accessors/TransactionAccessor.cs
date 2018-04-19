using Core;
using Core.DTOs;
using Core.Interfaces.Accessors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Accessors
{
    class TransactionAccessor : ITransactionAccessor
    {
        private readonly string _connectionString;

        public TransactionAccessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<Transaction> GetAllTransactionsForUser(int userID)
        {
            string query = "[dbo].[GetAllTransactionsForUser]";

            var result = new List<Transaction>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.Add(new SqlParameter("@UserID", userID));

                command.CommandType = System.Data.CommandType.StoredProcedure;

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
                        ReasonFailed = reader.IsDBNull(6) ? null : reader.GetString(6)
                    });
                }
            }
            return result;
        }

        public IList<Transaction> GetAllUnsettledTransactions()
        {
            string query = "[dbo].[GetAllUnsettledTransactions]";

            var result = new List<Transaction>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

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
                        ReasonFailed = reader.IsDBNull(6) ? null : reader.GetString(6)
                    });
                }
            }
            return result;
        }

        public IList<Transaction> GetAllFailedTransactions()
        {
            string query = "[dbo].[GetAllFailedTransactions]";

            var result = new List<Transaction>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

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
                        ReasonFailed = reader.IsDBNull(6) ? null : reader.GetString(6)
                    });
                }
            }

            return result;
        }

        public IList<TransactionWithUserInfoDTO> GetTransactionsForDateRange(DateTime startDate, DateTime endDate)
        {
            string query = "[dbo].[GetAllTransactionsForDateRange]";

            var result = new List<TransactionWithUserInfoDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                command.Parameters.Add(new SqlParameter("@StartDate", startDate.Date));
                command.Parameters.Add(new SqlParameter("@EndDate", endDate.Date));

                command.CommandType = System.Data.CommandType.StoredProcedure;

                conn.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new TransactionWithUserInfoDTO()
                    {
                        TransactionID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        AmountCharged = reader.GetDouble(4),
                        DateDue = reader.GetDateTime(5),
                        DateCharged = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                        ProcessState = ((ProcessState)reader.GetByte(7)).ToString(),
                        ReasonFailed = reader.IsDBNull(8) ? null : reader.GetString(8)
                    });
                }
            }
            return result;
        }

        public void AddTransaction(Transaction transaction)
        {
            string query = "[dbo].[AddTransaction]";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@UserID", transaction.UserID));
                command.Parameters.Add(new SqlParameter("@AmountCharged", transaction.AmountCharged));
                command.Parameters.Add(new SqlParameter("@DateDue", transaction.DateDue));
                command.Parameters.Add(new SqlParameter("@DateCharged", transaction.DateCharged));
                command.Parameters.Add(new SqlParameter("@ProcessState", transaction.ProcessState));
                command.Parameters.Add(new SqlParameter("@ReasonFailed", transaction.ReasonFailed));

                command.CommandType = CommandType.StoredProcedure;

                con.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    transaction.TransactionID = (int)reader.GetDecimal(0);
                }
            }
        }

        public void UpdateTransaction(Transaction transaction)
        {
            string query = "[dbo].[UpdateTransaction]";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@TransactionID", transaction.TransactionID));
                command.Parameters.Add(new SqlParameter("@UserID", transaction.UserID));
                command.Parameters.Add(new SqlParameter("@AmountCharged", transaction.AmountCharged));
                command.Parameters.Add(new SqlParameter("@DateDue", transaction.DateDue));
                command.Parameters.Add(new SqlParameter("@DateCharged", transaction.DateCharged));
                command.Parameters.Add(new SqlParameter("@ProcessState", transaction.ProcessState));
                command.Parameters.Add(new SqlParameter("@ReasonFailed", transaction.ReasonFailed));

                command.CommandType = CommandType.StoredProcedure;

                con.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    throw new Exception("Incorrect number of rows affected: " + rowsAffected);
                }
            }
        }
    }
}
