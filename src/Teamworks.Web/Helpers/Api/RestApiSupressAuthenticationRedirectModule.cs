using System;
using System.Web;

namespace Teamworks.Web.Helpers.Api
{
    public class RestApiSupressAuthenticationRedirectModule : IHttpModule
    {
        private const string SupressRedirectLoginKey = "WEBAPI_AUTHENTICATION";

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
        
    }
}