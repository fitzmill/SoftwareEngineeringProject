using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accessors
{
    public class SetReportAccessor : ISetReportAccessor
    {
        string connectionString;

        public SetReportAccessor(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void InsertReport(Report report)
        {
            string query = "[dbo].[InsertReport]";

            using (SqlConnection conn = new SqlConnection(connectionString))
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
