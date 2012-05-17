﻿using System;
using System.Web;

namespace Teamworks.Web.Helpers
{
    public class RestApiSupressAuthenticationRedirectModule : IHttpModule
    {
        private const string SupressRedirectLoginKey = "WEBAPI:Authentication";

        #region IHttpModule Members

        /// <summary>
        /// You will need to configure this module in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>

        #region IHttpModule Members
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PostReleaseRequestState += OnPostReleaseRequestState;
            context.EndRequest += OnEndRequest;
        }

        #endregion

        private void OnEndRequest(object sender, EventArgs eventArgs)
        {
            var context = (HttpApplication) sender;
            HttpResponse response = context.Response;

            if (!context.Context.Items.Contains(SupressRedirectLoginKey))
            {
                return;
            }

            response.TrySkipIisCustomErrors = true;
            response.ClearContent();
            response.StatusCode = 401;
            response.RedirectLocation = null;
        }

        private void OnPostReleaseRequestState(object sender, EventArgs args)
        {
            var context = (HttpApplication) sender;
            HttpRequest request = context.Request;
            if (!request.Url.LocalPath.StartsWith("/api"))
            {
                return;
            }

            HttpResponse response = context.Response;
            if (response.StatusCode == 401)
            {
                context.Context.Items[SupressRedirectLoginKey] = true;
            }
        }

        #endregion
    }
}