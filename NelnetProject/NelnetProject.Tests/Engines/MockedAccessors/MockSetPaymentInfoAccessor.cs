using Core.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    class MockSetPaymentInfoAccessor : ISetPaymentInfoAccessor
    {
        private static Random random = new Random();
        private readonly string alphaNumeric = "abcdefghijklmnopqrstuvwxyz0123456789";

        public List<UserPaymentInfoDTO> mockPaymentSpring;

        public MockSetPaymentInfoAccessor(List<UserPaymentInfoDTO> mockPaymentSpring)
        {
            this.mockPaymentSpring = mockPaymentSpring;
        }

        public string CreateCustomer(UserPaymentInfoDTO customerInfo)
        {
            string customerID = new string(Enumerable.Repeat(alphaNumeric, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            while (mockPaymentSpring.Select(info => info.CustomerID).Contains(customerID))
            {
                customerID = new string(Enumerable.Repeat(alphaNumeric, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            customerInfo.CustomerID = customerID;

            mockPaymentSpring.Add(customerInfo);
            return customerID;
        }

        public void DeleteCustomer(string customerID)
        {
            mockPaymentSpring.RemoveAll(info => info.CustomerID.Equals(customerID));
        }

        public void UpdateCustomer(UserPaymentInfoDTO customerInfo)
        {
            mockPaymentSpring.RemoveAll(info => info.CustomerID.Equals(customerInfo.CustomerID));
            mockPaymentSpring.Add(customerInfo);
        }
    }
}
