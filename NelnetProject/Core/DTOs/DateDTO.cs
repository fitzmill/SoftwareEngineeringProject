using System.ComponentModel.DataAnnotations;

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
    }
}
