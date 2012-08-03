using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client.Authorization;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Api;

namespace Teamworks.Web.Attributes.Api
{
    public class SecureForAttribute : AuthorizeAttribute
    {
        public string Operation { get; set; }

        public SecureForAttribute()
        {
            Operation = Global.Constants.Operation;
        }


        public SecureForAttribute(string operation)
        {
            Operation = operation;
        }

        public override void OnAuthorization(HttpActionContext context)
        {
            var id = context.Request.GetCurrentPersonId();
            if (!string.IsNullOrEmpty(id))
            {
                var session = context.Request.GetOrOpenSession();
                session.SecureFor(id, Operation);
            }
            base.OnAuthorization(context);
        }
    }
}