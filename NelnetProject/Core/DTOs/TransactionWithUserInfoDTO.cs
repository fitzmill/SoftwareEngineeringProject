﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class TransactionWithUserInfoDTO
    {
        public int TransactionID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double AmountCharged { get; set; }

        public DateTime DateDue { get; set; }

        public DateTime? DateCharged { get; set; }

        public ProcessState ProcessState { get; set; }

        public ReasonFailed? ReasonFailed { get; set; }
    }
}