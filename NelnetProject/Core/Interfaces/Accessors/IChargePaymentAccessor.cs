using Core.DTOs;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface for charging payments to PaymentSpring.
    /// </summary>
    public interface IChargePaymentAccessor
    {
        /// <summary>
        /// Charges a customer through PaymentSpring.
        /// </summary>
        /// <param name="payment">The payment DTO, containing necessary information to charge through PaymentSpring</param>
        /// <returns>The charge result from PaymentSpring</returns>
        ChargeResultDTO ChargeCustomer(PaymentDTO payment);
    }
}
