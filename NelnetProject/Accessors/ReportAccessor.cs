using Core.Interfaces.Accessors;
using Core.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Accessors
{
    class ReportAccessor : IReportAccessor
    {
        private readonly string _connectionString;

        public ReportAccessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Report> GetAllReports()
        {
            string query = "[dbo].[GetAllReports]";

            var result = new List<Report>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
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

        public void InsertReport(Report report)
        {
            string query = "[dbo].[InsertReport]";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, conn);

                command.Parameters.Add(new SqlParameter("@StartDate", report.StartDate));
                command.Parameters.Add(new SqlParameter("@EndDate", report.EndDate));

                command.CommandType = System.Data.CommandType.StoredProcedure;

                conn.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    report.ReportID = (int)reader.GetDecimal(0);
                }
            }
        }
    }
}
