using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Helpers
{
    public class MailgunModelBinderProvider : ModelBinderProvider
    {
        private readonly MailgunModelBinder _modelBinder = new MailgunModelBinder();

        public override IModelBinder GetBinder(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(Mailgun) || 
                bindingContext.ModelType.IsSubclassOf(typeof(Dictionary<string, string>)))
            {
                return _modelBinder;
            }

            return null;
        }
    }
}