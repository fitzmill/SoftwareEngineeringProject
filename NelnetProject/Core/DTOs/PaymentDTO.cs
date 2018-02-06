using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class PaymentDTO
    {
        public string Username { get; set; }

        public int CustomerID { get; set; }

        public double Amount { get; set; }

        public bool SendReceipt { get; set; }
    }
}
