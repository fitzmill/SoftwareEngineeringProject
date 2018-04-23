using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    /// <summary>
    /// Model for transactions
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Transaction's ID
        /// </summary>
        [Range(0, int.MaxValue)]
        public int TransactionID { get; set; }

        /// <summary>
        /// User ID Associated With The Transaction
        /// </summary>
        [Range(1, int.MaxValue)]
        public int UserID { get; set; }

        /// <summary>
        /// Amount Charged In The Transaction
        /// </summary>
        [Range(0.0, double.MaxValue)]
        public double AmountCharged { get; set; }

        /// <summary>
        /// Due Date Of Transaction
        /// </summary>
        public DateTime DateDue { get; set; }

        /// <summary>
        /// Date The Transaction Is Charged
        /// </summary>
        public DateTime? DateCharged { get; set; }

        /// <summary>
        /// State That The Transaction Is In (NOT_YET_CHARGED, SUCCESSFUL, RETRYING, FAILED, DEFERRED)
        /// </summary>
        public ProcessState ProcessState { get; set; }

        /// <summary>
        /// States That A Transaction Failed Given By PaymentSpring
        /// </summary>
        public string ReasonFailed { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            var transaction = obj as Transaction;
            return transaction != null &&
                   TransactionID == transaction.TransactionID &&
                   UserID == transaction.UserID &&
                   AmountCharged == transaction.AmountCharged &&
                   DateDue == transaction.DateDue &&
                   EqualityComparer<DateTime?>.Default.Equals(DateCharged, transaction.DateCharged) &&
                   ProcessState == transaction.ProcessState &&
                   ReasonFailed == transaction.ReasonFailed;
        }

        public override int GetHashCode()
        {
            var hashCode = -635102645;
            hashCode = hashCode * -1521134295 + TransactionID.GetHashCode();
            hashCode = hashCode * -1521134295 + UserID.GetHashCode();
            hashCode = hashCode * -1521134295 + AmountCharged.GetHashCode();
            hashCode = hashCode * -1521134295 + DateDue.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTime?>.Default.GetHashCode(DateCharged);
            hashCode = hashCode * -1521134295 + ProcessState.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReasonFailed);
            return hashCode;
        }
    }
}
