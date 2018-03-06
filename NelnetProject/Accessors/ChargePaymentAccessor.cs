using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private HttpClientBuilder clientBuilder;
        private string urlBase;

        public ChargePaymentAccessor(HttpClientBuilder clientBuilder, string url)
        {
            this.clientBuilder = clientBuilder;
            urlBase = url;
        }

        public ChargeResultDTO ChargeCustomer(PaymentDTO payment)
        {
            string chargeUrl = urlBase + "/charge";

            using (HttpClient client = clientBuilder.BuildClientWithPrivateKey())
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

                return new ChargeResultDTO()
                {
                    WasSuccessful = result.settled,
                    ErrorMessage = result.error_message
                };
            }
        }
    }
}
