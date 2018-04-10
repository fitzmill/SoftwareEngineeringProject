using Core.DTOs;
using Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;



namespace Accessors
{
    public class SetPaymentInfoAccessor : ISetPaymentInfoAccessor
    {
        private HttpClientBuilder clientBuilder;
        private string urlBase; 

        public SetPaymentInfoAccessor(HttpClientBuilder clientBuilder, string url)
        {
            this.clientBuilder = clientBuilder;
            this.urlBase = url;
        }

        //create a customer through the paymentSpring API and return the generated customerID
        public string CreateCustomer(UserPaymentInfoDTO customerInfo)
        {
            string createCustomerUrl = this.urlBase + "/customers";

            using (HttpClient client = clientBuilder.BuildClientWithPrivateKey())
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

        //updates solely the customer's name and address related billing information
        public void UpdateCustomerBillingInformation(PaymentAddressDTO paymentAddressInfo)
        {
            string updateCustomerUrl = urlBase + "/customers/" + paymentAddressInfo.CustomerID;

            using (HttpClient client = clientBuilder.BuildClientWithPrivateKey())
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

        //updates solely the customer's card information
        public void UpdateCustomerCardInformation(PaymentCardDTO paymentCardInfo)
        {
            string updateCustomerUrl = urlBase + "/customers/" + paymentCardInfo.CustomerID;

            using (HttpClient client = clientBuilder.BuildClientWithPrivateKey())
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

        //delete the customer information from paymentSpring
        public void DeleteCustomer(string customerID)
        {
            string deleteCustomerUrl = this.urlBase + "/customers";

            using (HttpClient client = clientBuilder.BuildClientWithPrivateKey())
            {
                var values = new Dictionary<string, string>
                {
                    {"id", customerID}
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                Task<HttpResponseMessage> response = client.PostAsync(deleteCustomerUrl, content);
            }
        }
    }
}
