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
        public static string Send(string from, string to, string subject, string text)
        {
            return Send(from, to, subject, text, null);
        }

        public static string Send(string from, string to, string subject, string text, string id)
        {
            var message = new Dictionary<string, string>()
                              {
                                  {"to", MailgunConfiguration.Host},
                                  {"bcc", to},
                                  {"from", from},
                                  {"subject", subject},
                                  {"text", text}
                              };

            if(!string.IsNullOrEmpty(id))
                message.Add("h:message-id", id);

            var client = CreateClient();
            var content = new FormUrlEncodedContent(message);

            var result =
                client.PostAsync(client.BaseAddress + "/messages", content).Result;

            //change content type to JSON so we can parse the response
            result.Content.Headers.ContentType.MediaType = "application/json";
            
            var json = result.Content.ReadAsStringAsync().
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