using System;
using Raven.Client;
using System.Collections.Concurrent;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Api;

namespace Teamworks.Web.Attributes.Api
{
    public class RavenSessionAttribute : ActionFilterAttribute
    {
        private static readonly ConcurrentDictionary<Type, Accessors> AccessorsCache
            = new ConcurrentDictionary<Type, Accessors>();

        private static Accessors CreateAccessorsForType(Type type)
        {
            var prop = type.GetProperties().FirstOrDefault(
                    x => x.PropertyType == typeof(IDocumentSession) && x.CanRead && x.CanWrite);
            
            if (prop == null)
                return null;

            return new Accessors
            {
                Set = (instance, session) => prop.SetValue(instance, session, null),
                Get = instance => (IDocumentSession) prop.GetValue(instance, null)
            };
        }

        private class Accessors
        {
            public Action<object, IDocumentSession> Set;
            public Func<object, IDocumentSession> Get;
        }

        public static void TrySetSession(object instance, IDocumentSession session)
        {
            var accessors = AccessorsCache.GetOrAdd(instance.GetType(), CreateAccessorsForType);

            if (accessors == null)
                return;

            accessors.Set(instance, session);
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            var session = context.Request.Properties[App.Keys.RavenDbSessionKey] as IDocumentSession;
            TrySetSession(context.ControllerContext.Controller, session);
            base.OnActionExecuting(context);
        }
    }
}