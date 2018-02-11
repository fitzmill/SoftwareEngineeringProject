using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Transaction
    {
        public int TransactionID { get; set; }

        public int UserID { get; set; }

        public double AmountCharged { get; set; }

        public DateTime DateDue { get; set; }

        public DateTime? DateCharged { get; set; }

        public ProcessState ProcessState { get; set; }

        public ReasonFailed? ReasonFailed { get; set; }
    }
}
