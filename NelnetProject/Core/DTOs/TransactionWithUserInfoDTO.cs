using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            var dTO = obj as TransactionWithUserInfoDTO;
            return dTO != null &&
                   TransactionID == dTO.TransactionID &&
                   FirstName == dTO.FirstName &&
                   LastName == dTO.LastName &&
                   Email == dTO.Email &&
                   AmountCharged == dTO.AmountCharged &&
                   DateDue == dTO.DateDue &&
                   EqualityComparer<DateTime?>.Default.Equals(DateCharged, dTO.DateCharged) &&
                   ProcessState == dTO.ProcessState &&
                   ReasonFailed == dTO.ReasonFailed;
        }

        public override int GetHashCode()
        {
            var hashCode = 1531269167;
            hashCode = hashCode * -1521134295 + TransactionID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            hashCode = hashCode * -1521134295 + AmountCharged.GetHashCode();
            hashCode = hashCode * -1521134295 + DateDue.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTime?>.Default.GetHashCode(DateCharged);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProcessState);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReasonFailed);
            return hashCode;
        }
    }
}
