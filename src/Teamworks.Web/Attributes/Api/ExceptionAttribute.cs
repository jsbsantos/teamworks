﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Teamworks.Web.Attributes.Api
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public ExceptionAttribute()
        {
            Mappings = new Dictionary<Type, Rule>();
        }

        public IDictionary<Type, Rule> Mappings { get; private set; }

        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                Exception exception = context.Exception;
                if (exception is HttpException)
                {
                    context.Response =
                        context.Request.CreateResponse((HttpStatusCode) ((HttpException) exception).GetHttpCode(),
                                                       (exception.Message));
                }
                else if (Mappings.ContainsKey(exception.GetType()))
                {
                    Rule rule = Mappings[exception.GetType()];
                    context.Response = rule.HasBody
                                           ? context.Request.CreateResponse(rule.Status, exception.Message)
                                           : context.Request.CreateResponse(rule.Status);
                }
                else if (!(exception is HttpResponseException))
                {
                   
#if DEBUG            
                    context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, exception);
#else
                    context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError);
#endif
                }
            }
            base.OnException(context);
        }

        #region Nested type: Rule

        public struct Rule
        {
            public bool HasBody;
            public HttpStatusCode Status;
        }

        #endregion
    }
}