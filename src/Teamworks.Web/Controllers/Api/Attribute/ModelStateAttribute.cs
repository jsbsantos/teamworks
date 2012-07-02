using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Teamworks.Web.Controllers.Api.Attribute
{
    public class ModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                dynamic dyn = new ExpandoObject();
                foreach (var entry in context.ModelState)
                {
                    ((IDictionary<string, object>) dyn).Add(entry.Key.Replace("model.", ""),
                                                            entry.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                }
                var response = new HttpResponseMessage<dynamic>(dyn, HttpStatusCode.BadRequest);
                throw new HttpResponseException(response);
            }
        }
    }
}