﻿using System.Collections.Generic;
using System.Web.Mvc;
using Raven.Client;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers
{
    [Authorize]
    public abstract class RavenController : Controller
    {
       
        public IDocumentSession DbSession { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            DbSession = context.HttpContext.GetCurrentRavenSession();
            base.OnActionExecuting(context);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewBag.Breadcrumb = CreateBreadcrumb();
            base.OnResultExecuting(filterContext);
        }
        
        protected override void OnResultExecuted(ResultExecutedContext context)
        {
            if ((context.Exception == null || context.ExceptionHandled) && DbSession != null)
            {
                using (var session = DbSession)
                {
                    session.SaveChanges();
                }
            }
            base.OnResultExecuted(context);
        }

        public virtual BreadcrumbViewModel[] CreateBreadcrumb()
        {
            return new BreadcrumbViewModel[0];
        }
    }
}