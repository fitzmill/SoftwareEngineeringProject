using Core.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accessors
{
    public class SetPaymentInfoAccessor : ISetPaymentInfoAccessor
    {
        public string CreateCustomer(UserPaymentInfoDTO customerInfo)
        {
            //create a customer through the paymentSpring API
            //return the generated customerID
            throw new NotImplementedException();
        }

        //TODO: we may need to pass in the customerID here in order to know which customer to update (unless our DTO has everything that paymentSpring needs)
        public void UpdateCustomer(UserPaymentInfoDTO customerInfo) 
        {
            //update the customer information in paymentSpring
            throw new NotImplementedException();
        }
    }
}
