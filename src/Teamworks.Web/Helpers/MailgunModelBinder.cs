using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Teamworks.Web.Models;

namespace Teamworks.Web.Helpers
{
    public class MailgunModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (actionContext.RequestContentKeyValueModel != null)
            {
                var model = new MailgunModel();
                foreach (var key in actionContext.RequestContentKeyValueModel.Keys)
                {
                    model[key.ToLower()] = bindingContext.ValueProvider.GetValue(key).AttemptedValue;
                }
                bindingContext.Model = model;
                return true;
            }

            return false;
        }
    }
}