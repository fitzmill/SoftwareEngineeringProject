using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core
{
    public class Transaction
    {
        [Range(1, int.MaxValue)]
        public int TransactionID { get; set; }

        [Range(1, int.MaxValue)]
        public int UserID { get; set; }

        [Range(0.0, double.MaxValue)]
        public double AmountCharged { get; set; }

        public DateTime DateDue { get; set; }

        public DateTime? DateCharged { get; set; }

        public ProcessState ProcessState { get; set; }

        public string ReasonFailed { get; set; }

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
