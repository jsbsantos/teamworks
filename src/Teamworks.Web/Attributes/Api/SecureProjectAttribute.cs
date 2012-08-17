using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Extensions.Api;

namespace Teamworks.Web.Attributes.Api
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SecureProjectAttribute : SecureAttribute
    {
        public SecureProjectAttribute(string operation, string key = "projectId")
            : base(operation)
        {
            Key = key;
        }

        public string Key { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            int id;
            try
            {
                id = int.Parse(actionContext.Request.GetRouteData().Values[Key].ToString());
            }
            catch (Exception e)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);
                return;
            }

            var session = actionContext.Request.Properties[Application.Keys.RavenDbSessionKey] as IDocumentSession;
            session.Load<Core.Project>(id);
        }
    }
}