using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Authentication;
using Teamworks.Web.Controllers;
using Teamworks.Web.Unittest.Api.Fixture;
using Xunit;
using Xunit.Sdk;

namespace Teamworks.Web.Unittest.Api
{
    public abstract class BaseControllerTester : IUseFixture<ApplicationHelper>
    {
        public ApplicationHelper Configure { get; set; }

        #region IUseFixture<ApplicationHelper> Members

        public void SetFixture(ApplicationHelper raven)
        {
            Configure = raven;
        }

        #endregion

        protected T ControllerForTests<T>(IDocumentSession session, HttpMethod method) where T : RavenApiController
        {
            var person = Person.Forge("email@mail.pt", "username", "password", "Name");
            Thread.CurrentPrincipal = new GenericPrincipal(new PersonIdentity(person), new string[0]);
        
            var controller = Activator.CreateInstance(typeof (T)) as T;
            if (controller == null) 
                throw new NullException("controller");

            controller.DbSession = session;
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(method, Url);
            var routeData = RouteData(config);
            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

            return controller;
        }

        protected abstract string Url { get; }
        protected abstract IHttpRouteData RouteData(HttpConfiguration config);
    }
}