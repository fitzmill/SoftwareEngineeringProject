using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface for charging payments to PaymentSpring.
    /// </summary>
    public interface IChargePaymentAccessor
    {
        /// <summary>
        /// Charges a customer.
        /// </summary>
        /// <param name="payment">The payment DTO</param>
        /// <returns>The charge result from PaymentSpring</returns>
        ChargeResultDTO ChargeCustomer(PaymentDTO payment);
    }
}
