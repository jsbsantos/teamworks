using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Teamworks.Core.Mailgun
{
    public class MailHub
    {
        public static string Send(MailgunMessage message)
        {
            var client = CreateClient();
            var content = new FormUrlEncodedContent(message);
            var json =
                client.PostAsync(client.BaseAddress + "/messages", content).Result.Content.ReadAsAsync<string>().
                    Result;

            return JObject.Parse(json)["id"].Value<string>();
        }

        private static HttpClient CreateClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(MailgunConfiguration.Uri) };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", MailgunConfiguration.Credentials);
            return client;
        }
        
    }
}