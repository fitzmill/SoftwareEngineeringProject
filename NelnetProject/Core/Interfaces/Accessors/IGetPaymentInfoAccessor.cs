using Core.DTOs;

namespace Core.Interfaces
{
    /// <summary>
    /// Accessor for getting payment information from Payment Spring.
    /// </summary>
    public interface IGetPaymentInfoAccessor
    {
        /// <summary>
        /// Gets a user's payment info from PaymentSpring with their PaymentSpring Customer ID.
        /// </summary>
        /// <param name="customerID">The customer id provided by PaymentSpring associated with the customer</param>
        /// <returns>A user's payment info provided by PaymentSpring/returns>
        UserPaymentInfoDTO GetPaymentInfoForCustomer(string customerID);
    }
}
