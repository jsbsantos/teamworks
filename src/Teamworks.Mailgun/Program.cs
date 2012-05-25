using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Teamworks.Mailgun
{
    class Program
    {
        public static void Main(string[] args)
        {
            var basic = Encoding.ASCII.GetBytes("api:key-20kdn4zyszbkxvk63baq02sxppv2a4n5");

            var client = new HttpClient { BaseAddress = new Uri("https://api.mailgun.net/v2/teamworks.mailgun.org") };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(basic));

            var parameters = new Dictionary<string, string>
                                 {
                                     {"from", "Teamworks <notifications@teamworks.mailgun.org>"},
                                     {"to", "filipe.am.pinheiro@gmail.com"},
                                     {"subject", "Hello"},
                                     {"text", "Testing some Maigun awesomness!"} ,
                                     {"h:Message-Id", "c-000000000@teamworks.mailgun.org"} 
                                 };

            var content = new FormUrlEncodedContent(parameters);
            Console.WriteLine(client.PostAsync(client.BaseAddress + "/messages", content).Result.Content.ReadAsStringAsync().Result);
        }
    }
}
