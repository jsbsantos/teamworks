using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Helpers
{
    public class MailgunModelBinderProvider : ModelBinderProvider
    {
        private readonly MailgunModelBinder _modelBinder = new MailgunModelBinder();

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            if (modelType == typeof(Mailgun) ||
                modelType.IsSubclassOf(typeof(Dictionary<string, string>)))
            {
                return _modelBinder;
            }

            return null;
        }
    }
}