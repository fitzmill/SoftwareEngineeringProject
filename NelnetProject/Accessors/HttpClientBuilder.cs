using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Accessors
{
    /// <summary>
    /// Helper class to create an http client
    /// </summary>
    public class HttpClientBuilder
    {
        private readonly string _publicKey;
        private readonly string _privateKey;

        public HttpClientBuilder(string publicKey, string privateKey)
        {
            _publicKey = publicKey;
            _privateKey = privateKey;
        }

        /// <summary>
        /// Creates an HttpClient using basic authorization and adds the PaymentSpring public key as the username and a blank password
        /// </summary>
        /// <returns>An HttpClient that is authorized to access PaymentSpring with the public key</returns>
        public HttpClient BuildClientWithPublicKey()
        {
            HttpClient client = new HttpClient();
            string formatted = string.Format("{0}:{1}", _publicKey, "");
            string encrypted = Convert.ToBase64String(Encoding.ASCII.GetBytes(formatted));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encrypted);
            return client;
        }

        /// <summary>
        /// Creates an HttpClient using basic authorization and adds the PaymentSpring private key as the username and a blank password
        /// </summary>
        /// <returns>An HttpClient that is authorized to access PaymentSpring with the private key</returns>
        public HttpClient BuildClientWithPrivateKey()
        {
            HttpClient client = new HttpClient();
            string formatted = string.Format("{0}:{1}", _privateKey, "");
            string encrypted = Convert.ToBase64String(Encoding.ASCII.GetBytes(formatted));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encrypted);
            return client;
        }
    }
}
