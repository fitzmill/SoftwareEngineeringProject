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
        private string url; 

        public SetPaymentInfoAccessor(HttpClientBuilder clientBuilder, string url)
        {
            this.clientBuilder = clientBuilder;
            this.url = url;
        }

        //create a customer through the paymentSpring API and return the generated customerID
        public string CreateCustomer(UserPaymentInfoDTO customerInfo)
        {
            string createCustomerUrl = this.url + "/customers";

            using (HttpClient client = clientBuilder.BuildClientWithPrivateKey())
            {
                var values = new Dictionary<string, string>
                {
                    {"first_name", customerInfo.FirstName},
                    {"company", customerInfo.Company},
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

                customerInfo.CustomerID = result.id;

                return customerInfo.CustomerID;
            }
        }

        //update the customer information in paymentSpring
        public void UpdateCustomer(UserPaymentInfoDTO customerInfo) 
        {
            string updateCustomerUrl = url + "/customers/" + customerInfo.CustomerID;

            using (HttpClient client = clientBuilder.BuildClientWithPrivateKey())
            {
                var values = new Dictionary<string, string>
                {
                    {"first_name", customerInfo.FirstName},
                    {"company", customerInfo.Company},
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

                Task<HttpResponseMessage> response = client.PostAsync(updateCustomerUrl, content);
            }
        }
    }
}
