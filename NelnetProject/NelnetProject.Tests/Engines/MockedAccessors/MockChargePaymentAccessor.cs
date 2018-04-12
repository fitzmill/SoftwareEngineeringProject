using Core.DTOs;
using Core.Interfaces;
using System.Collections.Generic;

namespace NelnetProject.Tests.Engines
{
    internal class MockChargePaymentAccessor : IChargePaymentAccessor
    {
        public Dictionary<string, ChargeResultDTO> MockPaymentSpring = new Dictionary<string, ChargeResultDTO>();

        public ChargeResultDTO ChargeCustomer(PaymentDTO payment)
        {
            return MockPaymentSpring[payment.CustomerID];
        }
    }
}