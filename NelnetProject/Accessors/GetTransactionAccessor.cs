﻿using Core.Interfaces;
using System;
using System.Collections.Generic;
using Core;
using System.Data.SqlClient;
using Core.DTOs;

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
            string query = "[dbo].[GetAllTransactionsForUser]";

            //initialize return object
            var result = new List<Transaction>();

            //this makes the connection, but doesn't open it. The connection will automatically be disposed of at the end of the using braces
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //makes the SQL command with the query string and connection object
                SqlCommand command = new SqlCommand(query, conn);

                //replaces the @passedInUserID that's in the query string with the integer userID
                command.Parameters.Add(new SqlParameter("@UserID", userID));

                command.CommandType = System.Data.CommandType.StoredProcedure;

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
                        ReasonFailed = reader.IsDBNull(6) ? null : reader.GetString(6)
                    });
                }
            }
            return result;
        }

        //Executes a stored procedure in the database for getting all Transactions with ProcessState as a parameter
        public IList<Transaction> GetAllUnsettledTransactions()
        {
            //Gets all transactions that have not been marked successful
            string query = "[dbo].[GetAllUnsettledTransactions]";

            var result = new List<Transaction>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

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

        //Executes a stored procedure in the database for getting the most recent transaction with userID as a parameter
        public Transaction GetMostRecentTransactionForUser(int userID)
        {
            string query = "[dbo].[GetMostRecentTransactionForUser]";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@UserID", userID));

                command.CommandType = System.Data.CommandType.StoredProcedure;

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
                        ReasonFailed = reader.IsDBNull(6) ? null : reader.GetString(6)
                    };
                }      
            }

            return null;
        }

        //Executes a stored procedure in the database for getting all Transactions with startTime and endTime as parameters
        public IList<TransactionWithUserInfoDTO> GetTransactionsForDateRange(DateTime startDate, DateTime endDate)
        {
            string query = "[dbo].[GetAllTransactionsForDateRange]";

            var result = new List<TransactionWithUserInfoDTO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
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
                        AmountCharged = reader.GetDouble(3),
                        DateDue = reader.GetDateTime(4),
                        DateCharged = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                        ProcessState = (ProcessState)reader.GetByte(6),
                        ReasonFailed = reader.IsDBNull(7) ? null : reader.GetString(7)
                    });
                }
            }
            return result;
        }
    }
}
