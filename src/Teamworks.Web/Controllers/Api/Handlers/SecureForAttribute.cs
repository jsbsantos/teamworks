using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Raven.Client.Authorization;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Api;

namespace Teamworks.Web.Controllers.Api.Handlers
{
    public class SecureForAttribute : ActionFilterAttribute
    {
        public string Operation { get; set; }
        
        public SecureForAttribute(string operation)
        {
            Operation = operation;
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            var person = context.Request.GetCurrentPerson();

            var session = Global.Raven.CurrentSession;
            session.SecureFor(person.Id, Operation);
            
            base.OnActionExecuting(context);
        }

    }
}
