using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using Raven.Client;
using Teamworks.Web.Attributes.Api.Ordered;

namespace Teamworks.Web.Attributes.Api
{
    public class RavenSessionAttribute : OrderedActionFilterAttribute
    {
        private static readonly ConcurrentDictionary<Type, Accessors> AccessorsCache
            = new ConcurrentDictionary<Type, Accessors>();

        private static Accessors CreateAccessorsForType(Type type)
        {
            PropertyInfo prop = type.GetProperties().FirstOrDefault(
                x => x.PropertyType == typeof (IDocumentSession) && x.CanRead && x.CanWrite);

            if (prop == null)
                return null;

            return new Accessors
                       {
                           Set = (instance, session) => prop.SetValue(instance, session, null),
                           Get = instance => (IDocumentSession) prop.GetValue(instance, null)
                       };
        }

        public static void TrySetSession(object instance, IDocumentSession session)
        {
            Accessors accessors = AccessorsCache.GetOrAdd(instance.GetType(), CreateAccessorsForType);

            if (accessors == null)
                return;

            accessors.Set(instance, session);
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            var session = context.Request.Properties[Application.Keys.RavenDbSessionKey] as IDocumentSession;
            TrySetSession(context.ControllerContext.Controller, session);
            base.OnActionExecuting(context);
        }

        #region Nested type: Accessors

        private class Accessors
        {
            public Func<object, IDocumentSession> Get;
            public Action<object, IDocumentSession> Set;
        }

        #endregion
    }
}