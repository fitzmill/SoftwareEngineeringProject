using Core.DTOs;
using Core.Interfaces;

namespace NelnetProject.Tests.Engines
{
    internal class MockChargePaymentAccessor : IChargePaymentAccessor
    {
        public ChargeResultDTO ChargeCustomer(PaymentDTO payment)
        {
            throw new System.NotImplementedException();
        }
    }
}