using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Accessors
{
    public class HttpClientBuilder
    {
        private string publicKey;
        private string privateKey;

        public HttpClientBuilder(string publicKey, string privateKey)
        {
            this.publicKey = publicKey;
            this.privateKey = privateKey;
        }

        //creates an httpclient using basic authorization and adds the payment spring public key as the username and password as blank
        public HttpClient BuildClientWithPublicKey()
        {
            HttpClient client = new HttpClient();
            string formatted = string.Format("{0}:{1}", publicKey, "");
            string encrypted = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(formatted));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encrypted);
            return client;
        }

        //creates an httpclient using basic authorization and adds the payment spring private key as the username and password as blank
        public HttpClient BuildClientWithPrivateKey()
        {
            HttpClient client = new HttpClient();
            string formatted = string.Format("{0}:{1}", privateKey, "");
            string encrypted = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(formatted));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encrypted);
            return client;
        }

    }
}
