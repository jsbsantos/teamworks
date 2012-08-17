using System;
using System.Web.Http.Controllers;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Api.Ordered;
using Teamworks.Web.Helpers.Extensions.Api;

namespace Teamworks.Web.Attributes.Api
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SecureForAttribute : OrderedActionFilterAttribute
    {
        public SecureForAttribute()
        {
            Operation = Global.Constants.Operation;
        }


        public SecureForAttribute(string operation)
        {
            Operation = operation;
        }

        public string Operation { get; set; }

        public override void OnActionExecuting(HttpActionContext context)
        {
            string id = context.Request.GetCurrentPersonId();
            if (!string.IsNullOrEmpty(id))
            {
                var session = context.Request.Properties[Application.Keys.RavenDbSessionKey] as IDocumentSession;
                session.SecureFor(id, Operation);
            }
        }
    }
}