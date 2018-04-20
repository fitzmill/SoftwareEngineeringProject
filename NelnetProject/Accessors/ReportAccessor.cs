using Core.Interfaces.Accessors;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Accessors
{
    public class ReportAccessor : IReportAccessor
    {
        private readonly string _connectionString;

        public ReportAccessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        private readonly string _getAllReportsQuery = "SELECT * FROM [dbo].[Report] r ORDER BY r.ReportID DESC";
        private readonly string _insertReportQuery = "INSERT INTO [dbo].[Report] (DateCreated, StartDate, EndDate) VALUES (@DateCreated, @StartDate, @EndDate)";

        public IEnumerable<Report> GetAllReports()
        {
            var result = new List<Report>();

            SqlConnectionFactory.RunSqlQuery(_getAllReportsQuery, reader => {
                while (reader.Read())
                {
                    result.Add(new Report
                    {
                        ReportID = reader.GetInt32(0),
                        DateCreated = reader.GetDateTime(1),
                        StartDate = reader.GetDateTime(2),
                        EndDate = reader.GetDateTime(3)
                    });
                }
            });

            return result;
        }

        public void InsertReport(Report report)
        {
            var parms = new Dictionary<string, object>
            {
                { "@DateCreated", DateTime.Today },
                { "@StartDate", report.StartDate },
                { "@EndDate", report.EndDate }
            };

            SqlConnectionFactory.RunSqlQuery(_insertReportQuery, parms, reader =>
            {
                if (reader.Read())
                {
                    report.ReportID = (int)reader.GetDecimal(0);
                }
            });
        }
    }
}
