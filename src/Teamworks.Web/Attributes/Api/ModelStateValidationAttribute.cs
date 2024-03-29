﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Teamworks.Web.Attributes.Api
{
    public class ModelStateValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                IDictionary<string, string[]> dict = new Dictionary<string, string[]>();
                foreach (var entry in context.ModelState)
                {
                    dict.Add(entry.Key.Replace("model.", ""),
                             entry.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                }
                HttpResponseMessage response = context.Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                throw new HttpResponseException(response);
            }
        }
    }
}