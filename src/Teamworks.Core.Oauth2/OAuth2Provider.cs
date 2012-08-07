using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;

namespace Teamworks.Core.Oauth2
{
    public abstract class OAuth2Provider
    {
        protected string AccessToken { get; set; }
        protected DateTime ExpiresIn { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string Callback { get; set; }

        protected string Request(string method,
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
                                     string[] split = i.Split(new[] {'='});
                                     param.Add(split[0], split[1]);
                                 });

                var content = new FormUrlEncodedContent(param);
                HttpResponseMessage response = client.PostAsync(client.BaseAddress, content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            if (method == "GET")
            {
                foreach (string key in headers.AllKeys)
                    client.DefaultRequestHeaders.Add(key, headers[key]);
                HttpResponseMessage response = client.GetAsync(uri.Uri).Result;
                return response.Content.ReadAsStringAsync().Result;
            }

            throw new NotSupportedException("Method not supported.");
        }
    }
}