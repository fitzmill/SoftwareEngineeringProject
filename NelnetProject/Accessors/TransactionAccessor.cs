using Core;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces.Accessors;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Accessors
{
    public class TransactionAccessor : ITransactionAccessor
    {
        public IEnumerable<Transaction> GetAllTransactionsForUser(int userID)
        {
            string query = "SELECT * FROM [dbo].[Transaction] t WHERE t.UserID = @UserID";
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userID }
            };

            var result = new List<Transaction>();

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
                while (reader.Read())
                {
                    result.Add(TransactionFromReader(reader));
                }
            });

            return result;
        }

        public IEnumerable<Transaction> GetAllUnsettledTransactions()
        {
            string query = String.Format("SELECT * FROM [dbo].[Transaction] t " +
                            "WHERE t.ProcessState = {0:D} OR " +
                            "t.ProcessState = {1:D}", ProcessState.RETRYING, ProcessState.FAILED);
                            
            var result = new List<Transaction>();

            SqlConnectionFactory.RunSqlQuery(query, reader =>
            {
                while (reader.Read())
                {
                    result.Add(TransactionFromReader(reader));
                }
            });

            return result;
        }

        public IEnumerable<Transaction> GetAllFailedTransactions()
        {
            string query = "SELECT * FROM [dbo].[Transaction] t WHERE t.ProcessState = 4";

            var result = new List<Transaction>();

            SqlConnectionFactory.RunSqlQuery(query, reader =>
            {
                while (reader.Read())
                {
                    result.Add(TransactionFromReader(reader));
                }
            });

            return result;
        }

        public IEnumerable<TransactionWithUserInfoDTO> GetTransactionsForDateRange(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT t.TransactionID, u.FirstName, u.LastName, u.Email, t.AmountCharged, t.DateDue, t.DateCharged, t.ProcessState, t.ReasonFailed " +
                "FROM [dbo].[Transaction] t JOIN [dbo].[User] u ON t.UserID = u.UserID WHERE t.DateDue >= @StartDate AND t.DateDue <= @EndDate";
            var parameters = new Dictionary<string, object>
            {
                { "@StartDate", startDate },
                { "@EndDate", endDate }
            };

            var result = new List<TransactionWithUserInfoDTO>();

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
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
            });

            return result;
        }

        public void AddTransaction(Transaction transaction)
        {
            string query = "INSERT INTO [dbo].[Transaction] (UserID, AmountCharged, DateDue, DateCharged, ProcessState, ReasonFailed) " +
                "VALUES (@UserID, @AmountCharged, @DateDue, @DateCharged, @ProcessState, @ReasonFailed); SELECT SCOPE_IDENTITY()";
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", transaction.UserID },
                { "@AmountCharged", transaction.AmountCharged },
                { "@DateDue", transaction.DateDue },
                { "@DateCharged", transaction.DateCharged },
                { "@ProcessState", transaction.ProcessState },
                { "@ReasonFailed", transaction.ReasonFailed }
            };

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
                if (reader.Read())
                {
                    transaction.TransactionID = (int)reader.GetDecimal(0);
                }
            });
        }

        public void UpdateTransaction(Transaction transaction)
        {
            string query = "UPDATE [dbo].[Transaction] SET UserID = @UserID, AmountCharged = @AmountCharged, DateDue = @DateDue, DateCharged = @DateCharged, " +
                "ProcessState = @ProcessState, ReasonFailed = @ReasonFailed WHERE TransactionID = @TransactionID; SELECT SCOPE_IDENTITY()";
            var parameters = new Dictionary<string, object>
            {
                { "@TransactionID", transaction.TransactionID },
                { "@UserID", transaction.UserID },
                { "@AmountCharged", transaction.AmountCharged },
                { "@DateDue", transaction.DateDue },
                { "@DateCharged", transaction.DateCharged },
                { "@ProcessState", transaction.ProcessState },
                { "@ReasonFailed", transaction.ReasonFailed }
            };

            int rowsAffected = SqlConnectionFactory.RunSqlNonQuery(query, parameters);

            if (rowsAffected != 1)
            {
                throw new SqlRowNotAffectedException($"Given transaction did not affect 1 row. Rows affected: {rowsAffected}");
            }
        }

        /// <summary>
        /// Helper method to extract single a transaction from an SqlDataReader.
        /// </summary>
        /// <param name="reader">the SqlDataReader</param>
        /// <returns>the extracted transaction</returns>
        private Transaction TransactionFromReader(SqlDataReader reader)
        {
            return new Transaction
            {
                TransactionID = reader.GetInt32(0),
                UserID = reader.GetInt32(1),
                AmountCharged = reader.GetDouble(2),
                DateDue = reader.GetDateTime(3),
                DateCharged = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                ProcessState = (ProcessState)reader.GetByte(5),
                ReasonFailed = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }
    }
}
