using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    /// <summary>
    /// This is for importing a date from java script to be parsed into a date
    /// </summary>
    public class DateDTO
    {
        /// <summary>
        /// The year of the date
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The month of the date
        /// </summary>
        [Range(1, 12)]
        public int Month { get; set; }

        /// <summary>
        /// The day of the date
        /// </summary>
        [Range(1, 31)]
        public int Day { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            var dTO = obj as DateDTO;
            return dTO != null &&
                   Year == dTO.Year &&
                   Month == dTO.Month &&
                   Day == dTO.Day;
        }

        public override int GetHashCode()
        {
            var hashCode = 592158470;
            hashCode = hashCode * -1521134295 + Year.GetHashCode();
            hashCode = hashCode * -1521134295 + Month.GetHashCode();
            hashCode = hashCode * -1521134295 + Day.GetHashCode();
            return hashCode;
        }
    }
}
