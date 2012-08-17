﻿using System;
using System.Net;
using System.Net.Http;
using Raven.Client;
using Teamworks.Web.Attributes.Api.Ordered;

namespace Teamworks.Web.Attributes.Api
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class VetoProject : OrderedActionFilterAttribute
    {
        public VetoProject()
        {
            RouteValue = "projectId";
        }

        public string RouteValue { get; set; }

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            int id;
            try
            {
                id = int.Parse(actionContext.Request.GetRouteData().Values[RouteValue].ToString());
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