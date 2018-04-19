using Core.Interfaces;
using System;
using System.Collections.Generic;
using Core;
using System.Data.SqlClient;
using Core.DTOs;

namespace Accessors
{
    public class GetTransactionAccessor : IGetTransactionAccessor
    {
        private readonly string _connectionString;

        public GetTransactionAccessor(string connectionString)
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
    }
}
