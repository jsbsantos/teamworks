using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Raven.Client;
using Teamworks.Core;

namespace Teamworks.Web.Attributes.Api
{
    public class VetoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            int id;
            try
            {
                id = int.Parse(actionContext.Request.GetRouteData().Values["projectId"].ToString());
            }
            catch (Exception e)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest);
                return;
            }
            var session = actionContext.Request.Properties[Application.Keys.RavenDbSessionKey] as IDocumentSession;
            session.Load<Project>(id);
        }
    }
}