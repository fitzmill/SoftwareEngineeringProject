using Newtonsoft.Json;
using Core.DTOs;
using Core.Interfaces;
using System.Threading.Tasks;

namespace Accessors
{
    public class GetPaymentInfoAccessor : IGetPaymentInfoAccessor
    {
        private readonly HttpClientBuilder _clientBuilder;
        private readonly string _urlBase;
        private readonly string _customerExtention = "/customers/";

        public GetPaymentInfoAccessor(HttpClientBuilder clientBuilder, string urlBase)
        {
            _urlBase = urlBase;
            _clientBuilder = clientBuilder;
        }

        public UserPaymentInfoDTO GetPaymentInfoForCustomer(string customerID) {
            using (var client = _clientBuilder.BuildClientWithPrivateKey())
            {
                UserPaymentInfoDTO paymentInfo = new UserPaymentInfoDTO();
                Task<string> responseString = client.GetStringAsync($"{_urlBase}{_customerExtention}{customerID}");
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
    }
}