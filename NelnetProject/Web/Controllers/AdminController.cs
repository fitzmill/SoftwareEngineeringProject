using Accessors;
using Core;
using Core.Interfaces;
using Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Web.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        IGetTransactionEngine getTransactionEngine;
        public AdminController(IGetTransactionEngine getTransactionEngine)
        {
            this.getTransactionEngine = getTransactionEngine;
        }

        // GET api/admin/GetAllTransactionsForUser/5
        [Route("GetAllTransactionsForUser/{userID}")]
        public List<Transaction> GetAllTransactionsForUser(string userID)
        {
            int intUserID = 0;
            int.TryParse(userID, out intUserID);
            return getTransactionEngine.GetAllTransactionsForUser(intUserID);
        }
    }
}