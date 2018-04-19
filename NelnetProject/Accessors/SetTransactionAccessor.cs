using Core.Interfaces;
using System;
using Core;
using System.Data.SqlClient;
using System.Data;

namespace Accessors
{
    public class SetTransactionAccessor : ISetTransactionAccessor
    {

        private readonly string _connectionString;

        public SetTransactionAccessor(string connectionString)
        {
            _connectionString = connectionString;
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
