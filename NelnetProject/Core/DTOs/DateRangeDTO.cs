using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class DateRangeDTO
    {
        [Required]
        public DateDTO StartDate { get; set; }

        [Required]
        public DateDTO EndDate { get; set; }
    }
}
