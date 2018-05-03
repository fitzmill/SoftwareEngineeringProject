using Core.Interfaces.Accessors;
using Core.Models;
using System;
using System.Collections.Generic;

namespace Accessors
{
    public class ReportAccessor : IReportAccessor
    {
        public IEnumerable<Report> GetAllReports()
        {
            string query = "SELECT * FROM [dbo].[Report] r ORDER BY r.ReportID DESC";

            var result = new List<Report>();

            SqlConnectionFactory.RunSqlQuery(query, reader => {
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
            string query = "INSERT INTO [dbo].[Report] (DateCreated, StartDate, EndDate) VALUES (@DateCreated, @StartDate, @EndDate)";
            var parameters = new Dictionary<string, object>
            {
                { "@DateCreated", DateTime.Today },
                { "@StartDate", report.StartDate },
                { "@EndDate", report.EndDate }
            };

            SqlConnectionFactory.RunSqlQuery(query, parameters, reader =>
            {
                if (reader.Read())
                {
                    report.ReportID = (int)reader.GetDecimal(0);
                }
            });
        }
    }
}
