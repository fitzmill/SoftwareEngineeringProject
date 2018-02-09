using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    /// <summary>
    /// Accessor for getting payment information from Payment Spring.
    /// </summary>
    public interface IGetPaymentInfoAccessor
    {
        /// <summary>
        /// Gets a user's payment info from Payment Spring with their Payment Spring Customer ID.
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns>A user's payment info provided by Payment Spring/returns>
        UserPaymentInfoDTO GetPaymentInfoForCustomer(string customerID);
    }
}
