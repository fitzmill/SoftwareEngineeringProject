using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Interfaces;

namespace Accessors
{
    public class GetPaymentInfoAccessor : IGetPaymentInfoAccessor
    {
        // Gets a user's payment info from Payment Spring with their Payment Spring Customer ID.
        public UserPaymentInfoDTO GetPaymentInfoForCustomer(string customerID)
        {
            throw new NotImplementedException();
        }
    }
}
