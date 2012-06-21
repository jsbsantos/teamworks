using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Teamworks.Web.Controllers.Api.Filters
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public ExceptionAttribute()
        {
            Mappings = new Dictionary<Type, HttpStatusCode>();
        }

        public IDictionary<Type, HttpStatusCode> Mappings { get; private set; }

        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var exception = context.Exception;
                if (exception is HttpException)
                {
                    context.Result = new HttpResponseMessage<dynamic>(
                        new { exception.Message }, (HttpStatusCode) ((HttpException) exception).GetHttpCode());
                }
                else if (Mappings.ContainsKey(exception.GetType()))
                {
                    context.Result = new HttpResponseMessage<dynamic>(
                        new { exception.Message }, Mappings[exception.GetType()]);
                }
                else
                {
                    context.Result = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            base.OnException(context);
        }
    }
}