using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Api;

namespace Teamworks.Web.Attributes.Api
{
    public class SecureForAttribute : AuthorizeAttribute
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

        public override void OnAuthorization(HttpActionContext context)
        {
            string id = context.Request.GetCurrentPersonId();
            if (!string.IsNullOrEmpty(id))
            {
                var session = context.Request.Properties[Application.Keys.RavenDbSessionKey] as IDocumentSession;
                session.SecureFor(id, Operation);
            }
            base.OnAuthorization(context);
        }
    }
}