using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Teamworks.Core.Mailgun
{
    public class MailHub
    {
        public static void NoReply(string to, string subject, string text)
        {
            var noreply = ConfigurationManager.AppSettings["mailgun:no-reply"];
            Send(noreply, to, subject, text);
        }

        public static string Send(string from, string to, string subject, string text, string id = null)
        {
            var message = new Dictionary<string, string>
                              {
                                  {"to", MailgunConfiguration.Host},
                                  {"bcc", to},
                                  {"from", from},
                                  {"subject", subject},
                                  {"text", text}
                              };

            var test = !Convert.ToBoolean(ConfigurationManager.AppSettings["mailgun:enabled"]);
            if (test)
            {
                message.Add("o:testmode", "true");
            }
            if (!string.IsNullOrEmpty(id))
                message.Add("h:message-id", id);

            HttpClient client = new HttpClient { BaseAddress = new Uri(MailgunConfiguration.Uri) };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                                                                       MailgunConfiguration.Credentials); 
            var content = new FormUrlEncodedContent(message);

            HttpResponseMessage result =
                client.PostAsync(client.BaseAddress + "/messages", content).Result;

            if (!result.IsSuccessStatusCode)
            {
                var m = result.Content.ReadAsStringAsync().Result;

                throw new HttpRequestException(m);
            }
            //change content type to JSON so we can parse the response
            result.Content.Headers.ContentType.MediaType = "application/json";

            string json = result.Content.ReadAsStringAsync().
                Result;

            return JObject.Parse(json)["id"].Value<string>();
        }
    }
}