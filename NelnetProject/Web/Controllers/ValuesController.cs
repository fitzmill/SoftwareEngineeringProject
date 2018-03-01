using Accessors;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public string Get()
        {
            HttpClientBuilder clientBuilder = new HttpClientBuilder("", "test_5492eef99f856a22e6c989a2d8");
            SetPaymentInfoAccessor accessor = new SetPaymentInfoAccessor(clientBuilder, "https://api.paymentspring.com/api/v1");
            UserPaymentInfoDTO customerInfo = new UserPaymentInfoDTO()
            {
                CustomerID = "a72cbf",
                Company = "Garmin",
                FirstName = "Luke",
                LastName = "Hall",
                StreetAddress1 = "1234 Weastern Rd.",
                StreetAddress2 = "",
                City = "Olathe",
                State = "KS",
                Zip = "98723",
                CardNumber = 4111111111111111,
                ExpirationYear = 2018,
                ExpirationMonth = 08
            };
            accessor.UpdateCustomer(customerInfo);
            return "done";
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
