using Core.DTOs;
using Core.Interfaces.Accessors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Accessors
{
    public class PaymentAccessor : IPaymentAccessor
    {
        private readonly HttpClientBuilder _clientBuilder;
        private readonly string _urlBase;
        private readonly string _customerExtension = "/customers";
        private readonly string _chargeExtension = "/charge";

        public PaymentAccessor(HttpClientBuilder clientBuilder, string url)
        {
            _clientBuilder = clientBuilder;
            _urlBase = url;
        }

        public ChargeResultDTO ChargeCustomer(PaymentDTO payment)
        {
            string chargeUrl = $"{_urlBase}{_chargeExtension}";

            using (HttpClient client = _clientBuilder.BuildClientWithPrivateKey())
            {
                var values = new Dictionary<string, string>
                {
                    {"customer_id", payment.CustomerID },
                    {"amount", payment.Amount.ToString() },
                    {"send_receipt", payment.SendReceipt.ToString() }
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(values);
                Task<HttpResponseMessage> response = client.PostAsync(chargeUrl, content);
                Task<string> responseTask = response.Result.Content.ReadAsStringAsync();

                dynamic result = JsonConvert.DeserializeObject(responseTask.Result);

                if (result.errors != null && result.errors.Count > 0)
                {
                    return new ChargeResultDTO()
                    {
                        WasSuccessful = false,
                        ErrorMessage = result.errors[0].message
                    };
                }

                return new ChargeResultDTO()
                {
                    WasSuccessful = result.settled,
                    ErrorMessage = result.error_message
                };
            }
        }

        public string CreateCustomer(UserPaymentInfoDTO customerInfo)
        {
            string createCustomerUrl = $"{_urlBase}{_customerExtension}";

            using (HttpClient client = _clientBuilder.BuildClientWithPrivateKey())
            {
                var values = new Dictionary<string, string>
                {
                    {"first_name", customerInfo.FirstName},
                    {"last_name", customerInfo.LastName},
                    {"address_1", customerInfo.StreetAddress1},
                    {"address_2", customerInfo.StreetAddress2},
                    {"city", customerInfo.City},
                    {"state", customerInfo.State},
                    {"zip", customerInfo.Zip},
                    {"card_number", customerInfo.CardNumber.ToString()},
                    {"card_exp_month", customerInfo.ExpirationMonth.ToString()},
                    {"card_exp_year", customerInfo.ExpirationYear.ToString()}
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                Task<HttpResponseMessage> response = client.PostAsync(createCustomerUrl, content);

                Task<string> responseTask = response.Result.Content.ReadAsStringAsync();

                dynamic result = JsonConvert.DeserializeObject(responseTask.Result);

                return result.id;
            }
        }

        public void DeleteCustomer(string customerID)
        {
            string deleteCustomerUrl = $"{_urlBase}{_customerExtension}";

            using (HttpClient client = _clientBuilder.BuildClientWithPrivateKey())
            {
                var values = new Dictionary<string, string>
                {
                    {"id", customerID}
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                HttpResponseMessage response = client.PostAsync(deleteCustomerUrl, content).Result;
            }
        }

        public UserPaymentInfoDTO GetPaymentInfoForCustomer(string customerID)
        {
            using (var client = _clientBuilder.BuildClientWithPrivateKey())
            {
                UserPaymentInfoDTO paymentInfo = new UserPaymentInfoDTO();
                Task<string> responseString = client.GetStringAsync($"{_urlBase}{_customerExtension}/{customerID}");
                dynamic deserializedResponse = JsonConvert.DeserializeObject(responseString.Result);

                paymentInfo.FirstName = deserializedResponse.first_name;
                paymentInfo.LastName = deserializedResponse.last_name;
                paymentInfo.CardNumber = deserializedResponse.last_4;
                paymentInfo.CardType = deserializedResponse.card_type;
                paymentInfo.ExpirationMonth = deserializedResponse.card_exp_month;
                paymentInfo.ExpirationYear = deserializedResponse.card_exp_year;
                paymentInfo.StreetAddress1 = deserializedResponse.address_1;
                paymentInfo.StreetAddress2 = deserializedResponse.address_2;
                paymentInfo.City = deserializedResponse.city;
                paymentInfo.State = deserializedResponse.state;
                paymentInfo.Zip = deserializedResponse.zip;
                paymentInfo.CustomerID = customerID;

                return paymentInfo;
            }
        }

        public void UpdateCustomerBillingInformation(PaymentAddressDTO paymentAddressInfo)
        {
            string updateCustomerUrl = $"{_urlBase}{_customerExtension}/{paymentAddressInfo.CustomerID}";

            using (HttpClient client = _clientBuilder.BuildClientWithPrivateKey())
            {
                var values = new Dictionary<string, string>
                {
                    {"first_name", paymentAddressInfo.FirstName},
                    {"last_name", paymentAddressInfo.LastName},
                    {"address_1", paymentAddressInfo.StreetAddress1},
                    {"address_2", paymentAddressInfo.StreetAddress2},
                    {"city", paymentAddressInfo.City},
                    {"state", paymentAddressInfo.State},
                    {"zip", paymentAddressInfo.Zip}
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                HttpResponseMessage response = client.PostAsync(updateCustomerUrl, content).Result;
            }
        }

        public void UpdateCustomerCardInformation(PaymentCardDTO paymentCardInfo)
        {
            string updateCustomerUrl = $"{_urlBase}{_customerExtension}/{paymentCardInfo.CustomerID}";

            using (HttpClient client = _clientBuilder.BuildClientWithPrivateKey())
            {
                var values = new Dictionary<string, string>
                {
                    {"card_number", paymentCardInfo.CardNumber.ToString()},
                    {"card_exp_month", paymentCardInfo.ExpirationMonth.ToString()},
                    {"card_exp_year", paymentCardInfo.ExpirationYear.ToString()}
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                HttpResponseMessage response = client.PostAsync(updateCustomerUrl, content).Result;
            }
        }
    }
}
