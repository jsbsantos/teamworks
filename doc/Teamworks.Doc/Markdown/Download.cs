using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Teamworks.Doc.Markdown
{
    public class Download : IMarkdownHandler
    {
        private readonly string _folder;
        private const string Pattern = @"!\[(.*\\label{(?<imagename>.[^}]*)})\]\((?<imageuri>.*/[^.]*(.*))\)";


        public Download(string folder)
        {
            _folder = folder;
        }

        public string Handle(string input)
        {
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }

            return Regex.Replace(input, Pattern, match =>
                                               {
                                                   var url = match.Groups["imageuri"].Value;
                                                   var i = url.LastIndexOf('.');
                                                   var ext = i > 0 ? url.Substring(i, url.Length - i) : "";

                                                   var name = match.Groups["imagename"]
                                                                  .Value.Replace(":", "") + ext;

                                                   var file = Path.Combine(_folder, name);
                                                   DownloadImage(file, url);

                                                   return string.Format(@"![{0}]({1})", match.Groups[1].Value, 
                                                       file.Replace("\\", "/"));
                                               },
                          RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        private static void DownloadImage(string file, string url)
        {
            Debug.Assert(!string.IsNullOrEmpty(file));
            Debug.Assert(!string.IsNullOrEmpty(url));


            var client = new HttpClient();
            var result = client.GetAsync(url).Result;

            if (!result.IsSuccessStatusCode
                || !result.Content.Headers.ContentType.MediaType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            using (var input = result.Content.ReadAsStreamAsync().Result)
            {
                using (var output = File.OpenWrite(file))
                {
                    var buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = input.Read(buffer, 0, buffer.Length);
                        output.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
            }
        }
    }
}