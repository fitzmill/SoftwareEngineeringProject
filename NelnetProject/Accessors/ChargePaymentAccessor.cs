using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Interfaces;
using Newtonsoft.Json;

namespace Accessors
{
    /// <summary>
    /// Charges payments to PaymentSpring.
    /// </summary>
    public class ChargePaymentAccessor : IChargePaymentAccessor
    {
        private readonly HttpClientBuilder _clientBuilder;
        private readonly string _urlBase;

        public ChargePaymentAccessor(HttpClientBuilder clientBuilder, string url)
        {
            _clientBuilder = clientBuilder;
            _urlBase = url;
        }

        public ChargeResultDTO ChargeCustomer(PaymentDTO payment)
        {
            string chargeUrl = _urlBase + "/charge";

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
    }
}
