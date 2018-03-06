using Accessors;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Core.Interfaces;

namespace Web.Controllers
{
    public class ValuesController : ApiController
    {
        private IChargePaymentAccessor paymentAccessor;
        
        public ValuesController (IChargePaymentAccessor paymentAccessor)
        {
            this.paymentAccessor = paymentAccessor;
        }

        // GET api/values
        public string Get()
        {
            return "ok";
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public IHttpActionResult Post([FromBody]string value)
        {
            return Ok();
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
