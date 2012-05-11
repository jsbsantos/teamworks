using System.Web.Mvc;
using Teamworks.Core;
using Teamworks.Web.Controllers;
using Teamworks.Web.Controllers.Base;

namespace Teamworks.Web.Helpers.Filters {
    public class RavenInitializerFilter : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var controller = filterContext.Controller as RavenController;
            if (controller != null && controller.DbSession == null) {
                Global.Raven.TryOpen();
            }
        }
    }

    
}