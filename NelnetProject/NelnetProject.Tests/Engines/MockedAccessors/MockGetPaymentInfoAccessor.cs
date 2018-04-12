using Core.DTOs;
using Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockGetPaymentInfoAccessor : IGetPaymentInfoAccessor
    {
        public List<UserPaymentInfoDTO> MockPaymentSpring;
        public MockGetPaymentInfoAccessor(List<UserPaymentInfoDTO> MockPaymentSpring)
        {
            this.MockPaymentSpring = MockPaymentSpring;
        }
        public UserPaymentInfoDTO GetPaymentInfoForCustomer(string customerID)
        {
            return MockPaymentSpring.FirstOrDefault(x => x.CustomerID == customerID);
        }
    }
}
