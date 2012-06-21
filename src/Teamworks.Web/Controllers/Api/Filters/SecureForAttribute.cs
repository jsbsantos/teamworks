using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client.Authorization;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Api;

namespace Teamworks.Web.Controllers.Api.Filters
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
            var person = context.Request.GetCurrentPerson();

            var session = Global.Database.CurrentSession;
            session.SecureFor(person.Id, Operation);
            
            base.OnAuthorization(context);
        }
    }
}
