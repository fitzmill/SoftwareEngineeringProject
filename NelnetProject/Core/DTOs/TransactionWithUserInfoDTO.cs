using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class TransactionWithUserInfoDTO
    {
        public int TransactionID { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Email { get; set; }

        [Range(0, double.MaxValue)]
        public double AmountCharged { get; set; }

        public DateTime DateDue { get; set; }

        public DateTime? DateCharged { get; set; }

        [Required]
        public string ProcessState { get; set; }

        public string ReasonFailed { get; set; }
    }
}
