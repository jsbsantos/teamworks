﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Teamworks.Core.Mailgun
{
    public class MailHub
    {
        public string Send()
        {
            var client = CreateClient();
            var parameters = new Dictionary<string, string>
                                 {
                                     {"from", "Teamworks <notifications@teamworks.mailgun.org>"},
                                     {"to", "filipe.am.pinheiro@gmail.com"},
                                     {"subject", "Registration"},
                                     {"o:testmode", "true"},
                                     {"text", "Success!"}
                                 };

            var content = new FormUrlEncodedContent(parameters);
            var json =
                client.PostAsync(client.BaseAddress + "/messages", content).Result.Content.ReadAsAsync<string>().
                    Result;

            return JObject.Parse(json)["id"].Value<string>(); ;
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient { BaseAddress = new Uri("https://api.mailgun.net/v2/teamworks.mailgun.org") };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Credentials);
            return client;
        }

        private string _credentials;
        private string Credentials
        {
            get
            {
                if (_credentials == null)
                {
                    var basic = Encoding.UTF8.GetBytes("api:key-20kdn4zyszbkxvk63baq02sxppv2a4n5");
                    _credentials = Convert.ToBase64String(basic);
                }
                return _credentials;
            }
        }
    }
}