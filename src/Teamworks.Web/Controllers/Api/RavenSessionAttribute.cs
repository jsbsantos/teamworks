using System.Web.Http.Filters;

namespace Teamworks.Web.Controllers.Api {
    public class RavenSessionAttribute : ActionFilterAttribute {
        public override void OnActionExecuted(HttpActionExecutedContext context) {
            RavenDocumentHolderAndSessionHandler.SaveSessionIfAvailable(context.ActionContext.ControllerContext.Controller);
        }
    }
}