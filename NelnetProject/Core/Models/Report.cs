using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    /// <summary>
    /// Model for report information
    /// </summary>
    public class Report
    {
        /// <summary>
        /// ID of the Report
        /// </summary>
        public int ReportID { get; set; }

        /// <summary>
        /// Date Of Report Creation
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Report Start Date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Report End Date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            var report = obj as Report;
            return report != null &&
                   ReportID == report.ReportID &&
                   DateCreated == report.DateCreated &&
                   StartDate == report.StartDate &&
                   EndDate == report.EndDate;
        }

        public override int GetHashCode()
        {
            var hashCode = 807587774;
            hashCode = hashCode * -1521134295 + ReportID.GetHashCode();
            hashCode = hashCode * -1521134295 + DateCreated.GetHashCode();
            hashCode = hashCode * -1521134295 + StartDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EndDate.GetHashCode();
            return hashCode;
        }
    }
}
