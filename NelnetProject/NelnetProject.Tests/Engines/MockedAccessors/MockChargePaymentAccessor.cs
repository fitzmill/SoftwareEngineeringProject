using Core.DTOs;
using Core.Interfaces;
using System.Collections.Generic;

namespace NelnetProject.Tests.Engines
{
    internal class MockChargePaymentAccessor : IChargePaymentAccessor
    {
        public Dictionary<PaymentDTO, ChargeResultDTO> MockPaymentSpring;

        public ChargeResultDTO ChargeCustomer(PaymentDTO payment)
        {
            throw new System.NotImplementedException();
        }
    }
}