using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Accessor for getting payment information.
    /// </summary>
    public interface IGetPaymentInfoAccessor
    {
        /// <summary>
        /// Gets a user's payment info from Payment Spring with their user ID.
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns>A user's payment info provided by Payment Spring/returns>
        PaymentDTO GetPaymentInfoForUser(int customerID);
    }
}
