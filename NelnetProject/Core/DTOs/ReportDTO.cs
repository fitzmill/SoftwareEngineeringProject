using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class ReportDTO
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Revenue { get; set; }

        public double ChargedPayments { get; set; }

        public double OutstandingPayments { get; set; }

        public double Profit { get; set; }
    }
}
