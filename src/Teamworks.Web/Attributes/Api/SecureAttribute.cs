using System;
using System.Web.Http.Controllers;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Web.Attributes.Api.Ordered;
using Teamworks.Web.Helpers.Extensions.Api;

namespace Teamworks.Web.Attributes.Api
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SecureAttribute : OrderedActionFilterAttribute
    {
        public SecureAttribute(string operation)
        {
            Operation = operation;
        }

        public string Operation { get; set; }

        public override void OnActionExecuting(HttpActionContext context)
        {
            string id = context.Request.GetCurrentPersonId() ?? string.Empty;
            var session = context.Request.Properties[Application.Keys.RavenDbSessionKey] as IDocumentSession;
            session.SecureFor(id, Operation);
        }
    }
}