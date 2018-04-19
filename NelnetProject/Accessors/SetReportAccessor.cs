using Core.Interfaces;
using Core.Models;
using System.Data.SqlClient;

namespace Accessors
{
    public class SetReportAccessor : ISetReportAccessor
    {
        private readonly string _connectionString;

        public SetReportAccessor(string connectionString)
        {
            _connectionString = connectionString;
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
