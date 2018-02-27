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
        
        private string url = "https://api.paymentspring.com/api/v1/customers";

        //create a customer through the paymentSpring API and return the generated customerID
        public string CreateCustomer(UserPaymentInfoDTO customerInfo)
        {
            using (HttpClient client = new HttpClient())
            {
                string formatted = string.Format("{0}:{1}", "test_5492eef99f856a22e6c989a2d8", "");
                string encrypted = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(formatted));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encrypted);

                var values = new Dictionary<string, string>
                {
                    {"username", customerInfo.Username},
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

                Task<HttpResponseMessage> response = client.PostAsync(url, content);

                Task<string> responseTask = response.Result.Content.ReadAsStringAsync();

                dynamic result = JsonConvert.DeserializeObject(responseTask.Result);

                customerInfo.CustomerID = result.id;

                return customerInfo.CustomerID.ToString();
            }
        }

        public void UpdateCustomer(UserPaymentInfoDTO customerInfo) 
        {
            //update the customer information in paymentSpring
            throw new NotImplementedException();
        }
    }
}
