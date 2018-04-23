using Core.DTOs;
using Core.Interfaces.Accessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NelnetProject.Tests.Engines.MockedAccessors
{
    public class MockPaymentAccessor : IPaymentAccessor
    {
        public Dictionary<string, ChargeResultDTO> MockPaymentSpringCharges;
        public List<UserPaymentInfoDTO> MockPaymentSpringCustomers;

        private static Random random = new Random();
        private readonly string alphaNumeric = "abcdefghijklmnopqrstuvwxyz0123456789";

        public MockPaymentAccessor(Dictionary<string, ChargeResultDTO> chargeResults, 
            List<UserPaymentInfoDTO> userPaymentInfos)
        {
            MockPaymentSpringCharges = chargeResults;
            MockPaymentSpringCustomers = userPaymentInfos;
        }

        public ChargeResultDTO ChargeCustomer(PaymentDTO payment)
        {
            return MockPaymentSpringCharges[payment.CustomerID];
        }

        public string CreateCustomer(UserPaymentInfoDTO customerInfo)
        {
            string customerID = new string(Enumerable.Repeat(alphaNumeric, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            while (MockPaymentSpringCustomers.Select(info => info.CustomerID).Contains(customerID))
            {
                customerID = new string(Enumerable.Repeat(alphaNumeric, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            customerInfo.CustomerID = customerID;

            MockPaymentSpringCustomers.Add(customerInfo);
            return customerID;
        }

        public void DeleteCustomer(string customerID)
        {
            MockPaymentSpringCustomers.RemoveAll(info => info.CustomerID.Equals(customerID));
        }

        public UserPaymentInfoDTO GetPaymentInfoForCustomer(string customerID)
        {
            return MockPaymentSpringCustomers.FirstOrDefault(x => x.CustomerID == customerID);
        }

        public void UpdateCustomerBillingInformation(PaymentAddressDTO paymentAddressInfo)
        {
            var customer = MockPaymentSpringCustomers.Where(u => u.CustomerID == paymentAddressInfo.CustomerID).FirstOrDefault();
            MockPaymentSpringCustomers.RemoveAll(info => info.CustomerID.Equals(paymentAddressInfo.CustomerID));
            customer.FirstName = paymentAddressInfo.FirstName;
            customer.LastName = paymentAddressInfo.LastName;
            customer.State = paymentAddressInfo.State;
            customer.StreetAddress1 = paymentAddressInfo.StreetAddress1;
            customer.StreetAddress2 = paymentAddressInfo.StreetAddress2;
            customer.Zip = paymentAddressInfo.Zip;
            MockPaymentSpringCustomers.Add(customer);
        }

        public void UpdateCustomerCardInformation(PaymentCardDTO paymentCardInfo)
        {
            var customer = MockPaymentSpringCustomers.Where(u => u.CustomerID == paymentCardInfo.CustomerID).FirstOrDefault();
            MockPaymentSpringCustomers.RemoveAll(info => info.CustomerID.Equals(paymentCardInfo.CustomerID));
            customer.CardNumber = paymentCardInfo.CardNumber;
            customer.ExpirationMonth = paymentCardInfo.ExpirationMonth;
            customer.ExpirationYear = paymentCardInfo.ExpirationYear;
            MockPaymentSpringCustomers.Add(customer);
        }
    }
}
