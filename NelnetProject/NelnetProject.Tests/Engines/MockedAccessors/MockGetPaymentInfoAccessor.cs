using Core.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockGetPaymentInfoAccessor : IGetPaymentInfoAccessor
    {
        public List<UserPaymentInfoDTO> MockPaymentSpring = new List<UserPaymentInfoDTO>()
        {
            new UserPaymentInfoDTO()
            {
                CustomerID = "fed123",
                Company = "Martwall",
                FirstName = "John",
                LastName = "Smith",
                StreetAddress1 = "1223 West St",
                StreetAddress2 = "Apt. 3",
                City = "Missouri City",
                State = "Kansas",
                Zip = "67208",
                CardNumber = 1123,
                ExpirationYear = 2025,
                ExpirationMonth = 6,
                CardType = "visa"
            }
        };

        public UserPaymentInfoDTO GetPaymentInfoForCustomer(string customerID)
        {
            if (customerID.Equals(MockPaymentSpring[0].CustomerID))
            {
                return MockPaymentSpring[0];
            } else
            {
                return null;
            }
        }
    }
}
