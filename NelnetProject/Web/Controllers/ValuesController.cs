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
            SetPaymentInfoAccessor accessor = new SetPaymentInfoAccessor();
            UserPaymentInfoDTO customerInfo = new UserPaymentInfoDTO()
            {
                //CustomerID = "fe1686",
                Username = "test_5492eef99f856a22e6c989a2d8",
                Company = "Microsoft",
                FirstName = "Lucas",
                LastName = "Hall",
                StreetAddress1 = "2936 W Western Rd.",
                StreetAddress2 = "",
                City = "Lee's Summit",
                State = "MO",
                Zip = "64086",
                CardNumber = 4111111111111111,
                ExpirationYear = 2018,
                ExpirationMonth = 08
            };
            return accessor.CreateCustomer(customerInfo);
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
