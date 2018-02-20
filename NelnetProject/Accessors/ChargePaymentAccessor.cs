using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Interfaces;

namespace Accessors
{
    /// <summary>
    /// Charges payments to PaymentSpring.
    /// </summary>
    public class ChargePaymentAccessor : IChargePaymentAccessor
    {
        public ChargeResultDTO ChargeCustomer(PaymentDTO payment)
        {
            //Sends payment to PaymentSpring DTO
            throw new NotImplementedException();
        }
    }
}
