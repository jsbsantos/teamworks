using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client.Authorization;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Api;

namespace Teamworks.Web.Controllers.Api.Attribute
{
    public class SecureForAttribute : AuthorizeAttribute
    {
        public string Operation { get; set; }

        public SecureForAttribute(string operation)
        {
            Operation = operation;
        }

        public override void OnAuthorization(HttpActionContext context)
        {
            var id = context.Request.GetCurrentPersonId();
            if (!string.IsNullOrEmpty(id))
            {
                var session = Global.Database.CurrentSession;
                session.SecureFor(id, Operation);
            }
            base.OnAuthorization(context);
        }
    }
}