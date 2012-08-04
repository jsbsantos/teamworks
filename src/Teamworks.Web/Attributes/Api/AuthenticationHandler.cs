using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Raven.Client;
using Teamworks.Core;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Attributes.Api
{
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        public class Credentials
        {
            public string Username;
            public String Password;
        }

        public static Credentials GetBase64Credentials(string base64)
        {
            var basic = Convert.FromBase64String(base64);
            var credentials = Encoding.UTF8.GetString(basic).Split(':');

            if (credentials.Length == 2)
            {
                return new Credentials
                           {
                               Username = credentials[0],
                               Password = credentials[1]
                           };
            }
            return new Credentials();
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            var header = context.Request.Headers.Authorization;
            if (header != null && header.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                var credentials = GetBase64Credentials(header.Parameter);
                var session = context.Request.Properties[App.Keys.RavenDbSessionKey] as IDocumentSession;
                var person = session.Query<Person>().FirstOrDefault(
                    p => p.Username.Equals(credentials.Username, StringComparison.InvariantCultureIgnoreCase));

                if (person.IsThePassword(credentials.Password))
                {
                    var identity = new PersonIdentity(person);
                    Thread.CurrentPrincipal = new GenericPrincipal(identity, person.Roles.ToArray());
                }
            }

            base.OnActionExecuting(context);
        }
    }
}