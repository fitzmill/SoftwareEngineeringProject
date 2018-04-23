using System;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    /// <summary>
    /// DTO for holding transaction information and the user information associated with that transaction
    /// </summary>
    public class TransactionWithUserInfoDTO
    {
        /// <summary>
        /// ID Of The Transaction
        /// </summary>
        public int TransactionID { get; set; }

        /// <summary>
        /// User's First Name
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string FirstName { get; set; }

        /// <summary>
        /// User's Last Name
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string LastName { get; set; }

        /// <summary>
        /// User's Email
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Email { get; set; }

        /// <summary>
        /// Amount Charged With The Transaction
        /// </summary>
        [Range(0, double.MaxValue)]
        public double AmountCharged { get; set; }

        /// <summary>
        /// Transaction Due Date
        /// </summary>
        public DateTime DateDue { get; set; }

        /// <summary>
        /// Date Transaction Was Charged
        /// </summary>
        public DateTime? DateCharged { get; set; }

        /// <summary>
        /// State That The Transaction Is In (NOT_YET_CHARGED, SUCCESSFUL, RETRYING, FAILED, DEFERRED)
        /// </summary>
        [Required]
        public string ProcessState { get; set; }

        /// <summary>
        /// States That A Transaction Failed Given By PaymentSpring
        /// </summary>
        public string ReasonFailed { get; set; }
    }
}
