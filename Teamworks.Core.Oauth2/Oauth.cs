using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace Teamworks.Core.Oauth2
{
    public class Oauth
    {
        private static string _AuthorizeParams =
            "code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type={4}";

        private static string _RefreshParams =
            "client_id={0}&client_secret={1}&refresh_token={2}&grant_type=refresh_token";

        private static string _host = "https://accounts.google.com/o/oauth2/";

        public string InitialRequest
        {
            get { return _host + "auth?client_id={0}&redirect_uri={1}&scope={2}"; }
        }

        private string AccessToken { get; set; }
        private string TokenType { get; set; }
        private DateTime ExpiresIn { get; set; }
        private string RefreshToken { get; set; }

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
            var request = WebRequest.Create(host) as HttpWebRequest;
            request.Method = method;

            if (headers != null)
                request.Headers.Add(headers);

            if (!string.IsNullOrEmpty(parameters))
            {
                byte[] body = Encoding.UTF8.GetBytes(parameters);

                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.ContentLength = body.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                }
            }

            HttpWebResponse response;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException e)
            {
                throw new Exception(e.Message, e);
            }

            return parseResponse(response);
        }


        private void Request(string _params, string code, string grant)
        {
            var param = string.Format(_params, code, ClientId, Secret, Callback, grant);
            var host = _host + "token";
            /*
            var request = WebRequest.Create(_host + "token") as HttpWebRequest;
            request.Method = method;

            byte[] body = Encoding.UTF8.GetBytes(param);

            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.ContentLength = body.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
            }

            HttpWebResponse response;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException e)
            {
                throw new Exception(e.Message, e);
            }
            */

            var response = Request("POST", host, param, null);
            var json = JObject.Parse(response);

            AccessToken = json["access_token"].Value<string>();
            ExpiresIn = DateTime.Now.AddSeconds(int.Parse(json["expires_in"].Value<string>()));
            TokenType = json["token_type"].Value<string>();
        }


        public string GetProfile(string authorizationCode)
        {
            if (string.IsNullOrEmpty(AccessToken))
                Request(_AuthorizeParams, authorizationCode, "authorization_code");

            return Request("GET", "https://www.googleapis.com/oauth2/v1/userinfo?alt=json", null,
                                   new NameValueCollection()
                                       {
                                           {"Authorization", "Bearer " + AccessToken}
                                       });
        }

        private string parseResponse(HttpWebResponse response)
        {
            var builder = new StringBuilder();
            using (var stream = response.GetResponseStream())
            {
                int count = 0;
                var buffer = new byte[8192];
                do
                {
                    count = stream.Read(buffer, 0, buffer.Length);
                    if (count != 0)
                    {
                        builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                    }
                } while (count > 0);
            }

            return builder.ToString();
        }

    }
}