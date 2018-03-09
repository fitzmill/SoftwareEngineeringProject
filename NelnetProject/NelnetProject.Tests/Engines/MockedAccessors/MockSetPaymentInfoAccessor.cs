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

        public IList<UserPaymentInfoDTO> mockPaymentSpring = new List<UserPaymentInfoDTO>
        {
            new UserPaymentInfoDTO
            {
                CustomerID = "abcdef",
                Company = "Nelnet",
                FirstName = "George",
                LastName = "Curious",
                StreetAddress1 = "601 NE Robin St",
                StreetAddress2 = "",
                City = "New York",
                State = "NY",
                Zip = "10001",
                CardNumber = 123412341234,
                ExpirationYear = 20,
                ExpirationMonth = 12,
                CardType = "Visa"
            }
        };

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
            throw new NotImplementedException();
        }

        public void UpdateCustomer(UserPaymentInfoDTO customerInfo)
        {
            throw new NotImplementedException();
        }
    }
}
