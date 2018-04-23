using System.ComponentModel.DataAnnotations;

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
    }
}
