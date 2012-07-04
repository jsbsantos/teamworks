using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Abstractions.Extensions;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Helpers
{
    public class MailgunModelBinder : IModelBinder
    {

        //public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        //{
        //    if (actionContext.RequestContentKeyValueModel != null)
        //    {
        //        var model = new MailgunModel();
        //        foreach (var key in actionContext.RequestContentKeyValueModel.Keys)
        //        {
        //            model[key.ToLower()] = bindingContext.ValueProvider.GetValue(key).AttemptedValue;
        //        }
        //        bindingContext.Model = model;
        //        return true;
        //    }

        //    return false;
        //}

       
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var content = HttpUtility.ParseQueryString(actionContext.Request.Content.ReadAsStringAsync().Result);
            var model = new Mailgun();
            content.ToDictionary().ForEach(i => model.Add(i.Key.ToLower(), i.Value));
            bindingContext.Model = model;
            return true;
        }
    }
}