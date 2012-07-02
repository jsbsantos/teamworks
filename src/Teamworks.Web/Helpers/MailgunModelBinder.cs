using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Helpers
{
    public class MailgunModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var value = JObject.Parse(actionContext.Request.Content.ReadAsStringAsync().Result);

            var serializer = new JsonSerializer();
            var model = (Mailgun)serializer.Deserialize(new JTokenReader(value), typeof(Mailgun));

            bindingContext.Model = model;
            return true;
        }
    }
}