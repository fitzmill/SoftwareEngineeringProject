using Core.DTOs;
using Core.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Accessors
{
    public class SetPaymentInfoAccessor : ISetPaymentInfoAccessor
    {
        private readonly HttpClientBuilder _clientBuilder;
        private readonly string _urlBase;
        private readonly string _customerExtention = "/customers";

        public SetPaymentInfoAccessor(HttpClientBuilder clientBuilder, string url)
        {
            _clientBuilder = clientBuilder;
            _urlBase = url;
        }

        public string CreateCustomer(UserPaymentInfoDTO customerInfo)
        {
            string createCustomerUrl = $"{_urlBase}{_customerExtention}";

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

        public void UpdateCustomerBillingInformation(PaymentAddressDTO paymentAddressInfo)
        {
            string updateCustomerUrl = $"{_urlBase}{_customerExtention}/{paymentAddressInfo.CustomerID}";

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
            string updateCustomerUrl = $"{_urlBase}{_customerExtention}/{paymentCardInfo.CustomerID}";

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

        public void DeleteCustomer(string customerID)
        {
            string deleteCustomerUrl = $"{_urlBase}{_customerExtention}";

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
    }
}
