using System;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace Teamworks.Core.Oauth2
{
    public class Google : OAuth2Provider
    {
        private const string _AuthorizeParams =
            "code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type={4}";

        private const string _host = "https://accounts.google.com/o/oauth2/";

        public string Url
        {
            get
            {
                return string.Format(
                    "{0}auth?client_id={1}&redirect_uri={2}&scope=https://www.googleapis.com/auth/userinfo.profile+https://www.googleapis.com/auth/userinfo.email&response_type=code",
                    _host,
                    ClientId, Callback);
            }
        }

        public void Authorize(string _params, string code, string grant)
        {
            string param = string.Format(_params, code, ClientId, Secret, Callback, grant);
            string host = _host + "token";

            string response = Request("POST", host, param, null);
            JObject json = JObject.Parse(response);

            AccessToken = json["access_token"].Value<string>();
            ExpiresIn = DateTime.Now.AddSeconds(int.Parse(json["expires_in"].Value<string>()));
        }

        public string GetProfile(string authorizationCode)
        {
            if (string.IsNullOrEmpty(AccessToken) || ExpiresIn > DateTime.Now)
                Authorize(_AuthorizeParams, authorizationCode, "authorization_code");

            return Request("GET", "https://www.googleapis.com/oauth2/v1/userinfo?alt=json", null,
                           new NameValueCollection
                               {
                                   {"Authorization", "Bearer " + AccessToken}
                               });
        }
    }
}