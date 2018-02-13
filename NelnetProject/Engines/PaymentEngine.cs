using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Interfaces;

namespace Engines
{
    /// <summary>
    /// Charges payments to users.
    /// </summary>
    class PaymentEngine : IPaymentEngine
    {
        public List<Transaction> ChargePayments(List<Transaction> charges)
        {
            //Charges payments to PaymentSpring
            //Returns results for notifications
            throw new NotImplementedException();
        }

        public List<Transaction> GeneratePayments()
        {
            //Generates payments
            //Stores them in the database
            throw new NotImplementedException();
        }
    }
}
