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

        public List<UserPaymentInfoDTO> _mockPaymentSpring;

        public MockSetPaymentInfoAccessor(List<UserPaymentInfoDTO> mockPaymentSpring)
        {
            _mockPaymentSpring = mockPaymentSpring;
        }

        public string CreateCustomer(UserPaymentInfoDTO customerInfo)
        {
            string customerID = new string(Enumerable.Repeat(alphaNumeric, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            while (_mockPaymentSpring.Select(info => info.CustomerID).Contains(customerID))
            {
                customerID = new string(Enumerable.Repeat(alphaNumeric, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            customerInfo.CustomerID = customerID;

            _mockPaymentSpring.Add(customerInfo);
            return customerID;
        }

        public void DeleteCustomer(string customerID)
        {
            _mockPaymentSpring.RemoveAll(info => info.CustomerID.Equals(customerID));
        }

        public void UpdateCustomer(UserPaymentInfoDTO customerInfo)
        {
            _mockPaymentSpring.RemoveAll(info => info.CustomerID.Equals(customerInfo.CustomerID));
            _mockPaymentSpring.Add(customerInfo);
        }

        public void UpdateCustomerBillingInformation(PaymentAddressDTO paymentAddressInfo)
        {
            var customer = _mockPaymentSpring.Where(u => u.CustomerID == paymentAddressInfo.CustomerID).FirstOrDefault();
            _mockPaymentSpring.RemoveAll(info => info.CustomerID.Equals(paymentAddressInfo.CustomerID));
            customer.FirstName = paymentAddressInfo.FirstName;
            customer.LastName = paymentAddressInfo.LastName;
            customer.State = paymentAddressInfo.State;
            customer.StreetAddress1 = paymentAddressInfo.StreetAddress1;
            customer.StreetAddress2 = paymentAddressInfo.StreetAddress2;
            customer.Zip = paymentAddressInfo.Zip;
            _mockPaymentSpring.Add(customer);
        }

        public void UpdateCustomerCardInformation(PaymentCardDTO paymentCardInfo)
        {
            var customer = _mockPaymentSpring.Where(u => u.CustomerID == paymentCardInfo.CustomerID).FirstOrDefault();
            _mockPaymentSpring.RemoveAll(info => info.CustomerID.Equals(paymentCardInfo.CustomerID));
            customer.CardNumber = paymentCardInfo.CardNumber;
            customer.ExpirationMonth = paymentCardInfo.ExpirationMonth;
            customer.ExpirationYear = paymentCardInfo.ExpirationYear;
            _mockPaymentSpring.Add(customer);
        }
    }
}
