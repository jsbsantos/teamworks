using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Teamworks.Core.Oauth2
{
    public class Google
    {
        private const string _AuthorizeParams = "code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type={4}";

        private const string _host = "https://accounts.google.com/o/oauth2/";

        public string InitialRequest
        {
            get { return _host + "auth?client_id={0}&redirect_uri={1}&scope={2}"; }
        }

        private string AccessToken { get; set; }
        private DateTime ExpiresIn { get; set; }

        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string Callback { get; set; }

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

        private string Request(string method,
                               string host,
                               string parameters,
                               NameValueCollection headers)
        {
            var uri = new UriBuilder(host) {Query = parameters};
            var client = new HttpClient
                             {
                                 BaseAddress = uri.Uri,
                             };

            if (method == "POST")
            {
                var param = new Dictionary<string, string>();
                parameters
                    .Split(new[] {'&'})
                    .ToList()
                    .ForEach(i =>
                                 {
                                     var split = i.Split(new[] {'='});
                                     param.Add(split[0], split[1]);
                                 });

                var content = new FormUrlEncodedContent(param);
                var response = client.PostAsync(client.BaseAddress, content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            if (method == "GET")
            {
                foreach (var key in headers.AllKeys)
                    client.DefaultRequestHeaders.Add(key, headers[key]);
                var response = client.GetAsync(uri.Uri).Result;
                return response.Content.ReadAsStringAsync().Result;
            }

            throw new NotSupportedException("Method not supported.");
        }

        public void Authorize(string _params, string code, string grant)
        {
            var param = string.Format(_params, code, ClientId, Secret, Callback, grant);
            var host = _host + "token";

            var response = Request("POST", host, param, null);
            var json = JObject.Parse(response);

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