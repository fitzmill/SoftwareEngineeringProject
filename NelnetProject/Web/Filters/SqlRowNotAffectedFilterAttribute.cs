using Core.Exceptions;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Web.Filters
{
    public class SqlRowNotAffectedFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is SqlRowNotAffectedException)
            {
                Debug.WriteLine(context.Exception.ToString());
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}