﻿using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accessors
{
    public class GetReportAccessor : IGetReportAccessor
    {
        string connectionString;
        public GetReportAccessor(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Report> GetAllReports()
        {
            string query = "[dbo].[GetAllReports]";

            var result = new List<Report>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                conn.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new Report()
                    {
                        ReportID = reader.GetInt32(0),
                        DateCreated = reader.GetDateTime(1),
                        StartDate = reader.GetDateTime(2),
                        EndDate = reader.GetDateTime(3)
                    });
                }
            }

            return result;
        }
    }
}