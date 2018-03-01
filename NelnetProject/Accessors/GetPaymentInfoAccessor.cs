using System.Net.Http;
using Newtonsoft.Json;
using Core.DTOs;
using Core.Interfaces;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;

namespace Accessors
{
    public class GetPaymentInfoAccessor : IGetPaymentInfoAccessor
    {
        private string paymentSpringAPIURL;
        private string customerExtention = "/customers/";
        private string username;

        public GetPaymentInfoAccessor(string paymentSpringAPIURL, string username)
        {
            this.paymentSpringAPIURL = paymentSpringAPIURL;
            this.username = username;
        }
    // Gets a user's payment info from Payment Spring with their Payment Spring Customer ID.
    public UserPaymentInfoDTO GetPaymentInfoForCustomer(string customerID) {
            using (var client = new HttpClient())
            {
                string formatted = string.Format("{0}:{1}", username, "");
                string encrypted = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(formatted));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encrypted);
                UserPaymentInfoDTO paymentInfo = new UserPaymentInfoDTO();
                Task<string> responseString = client.GetStringAsync(paymentSpringAPIURL + customerExtention + customerID);
                dynamic deserializedResponse = JsonConvert.DeserializeObject(responseString.Result);
                paymentInfo.FirstName = deserializedResponse.first_name;
                paymentInfo.LastName = deserializedResponse.last_name;
                paymentInfo.Company = deserializedResponse.company;
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
    }
}