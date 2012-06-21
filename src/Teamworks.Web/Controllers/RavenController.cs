﻿using Raven.Client;
using System.Web.Mvc;
using Teamworks.Core.Services;

namespace Teamworks.Web.Controllers
{
    [Authorize]
    public class RavenController : Controller
    {
        public IDocumentSession DbSession
        {
            get { return Global.Database.CurrentSession; }
        }

        protected override void OnResultExecuted(ResultExecutedContext context)
        {
            if ((context.Exception == null || context.ExceptionHandled) && DbSession != null)
            {
                using (IDocumentSession session = DbSession)
                {
                    session.SaveChanges();
                }
            }
            base.OnResultExecuted(context);
        }
    }
}