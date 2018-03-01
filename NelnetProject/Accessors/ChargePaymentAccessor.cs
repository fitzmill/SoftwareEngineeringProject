using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Interfaces;

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
                    {"amount", payment.ToString() },
                    {"send_receipt", payment.SendReceipt.ToString() }

                    //todo - finish this
                }
            }

            return null;
        }
    }
}
