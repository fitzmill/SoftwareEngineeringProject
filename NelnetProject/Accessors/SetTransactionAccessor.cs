using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Core;
using System.Data.SqlClient;
using System.Data;

namespace Accessors
{
    public class SetTransactionAccessor : ISetTransactionAccessor
    {

        private string connectionString;

        public SetTransactionAccessor(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //Executes a stored procedure to insert a transaction with all of the Transaction object's properties as parameters
        public void AddTransaction(Transaction transaction)
        {
            string query = "[dbo].[AddTransaction]";
            using (SqlConnection con = new SqlConnection(connectionString))
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

        //Executes a stored procedure to update a transaction with all of the Transaction object's properties as parameters
        public void UpdateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
