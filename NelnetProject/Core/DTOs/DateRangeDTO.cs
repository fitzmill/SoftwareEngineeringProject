using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    /// <summary>
    /// A DTO used to hold dates for report generation.
    /// </summary>
    public class DateRangeDTO
    {
        /// <summary>
        /// Start date of the report period
        /// </summary>
        [Required]
        public DateDTO StartDate { get; set; }

        /// <summary>
        /// End date of the report period
        /// </summary>
        [Required]
        public DateDTO EndDate { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            var dTO = obj as DateRangeDTO;
            return dTO != null &&
                   EqualityComparer<DateDTO>.Default.Equals(StartDate, dTO.StartDate) &&
                   EqualityComparer<DateDTO>.Default.Equals(EndDate, dTO.EndDate);
        }

        public override int GetHashCode()
        {
            var hashCode = -1134829439;
            hashCode = hashCode * -1521134295 + EqualityComparer<DateDTO>.Default.GetHashCode(StartDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<DateDTO>.Default.GetHashCode(EndDate);
            return hashCode;
        }
    }
}
