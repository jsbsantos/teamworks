using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Teamworks.Doc.Markdown
{
    public class Download : IMarkdownHandler
    {
        private const string Pattern = @"!\[(.*\\label{(?<imagename>.[^}]*)})\]\((?<imageuri>.*/[^.]*(.*))\)";
        private readonly string _folder;


        public Download(string folder)
        {
            _folder = folder;
        }

        #region IMarkdownHandler Members

        public string Handle(string input)
        {
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }

            return Regex.Replace(input, Pattern, match =>
                                                     {
                                                         string url = match.Groups["imageuri"].Value;
                                                         int i = url.LastIndexOf('.');
                                                         string ext = i > 0 ? url.Substring(i, url.Length - i) : "";

                                                         string name = match.Groups["imagename"]
                                                                           .Value.Replace(":", "") + ext;

                                                         string file = Path.Combine(_folder, name);
                                                         DownloadImage(file, url);

                                                         return string.Format(@"![{0}]({1})", match.Groups[1].Value,
                                                                              file.Replace("\\", "/"));
                                                     },
                                 RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        #endregion

        private static void DownloadImage(string file, string url)
        {
            Debug.Assert(!string.IsNullOrEmpty(file));
            Debug.Assert(!string.IsNullOrEmpty(url));


            var client = new HttpClient();
            HttpResponseMessage result = client.GetAsync(url).Result;

            if (!result.IsSuccessStatusCode
                || !result.Content.Headers.ContentType.MediaType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            using (Stream input = result.Content.ReadAsStreamAsync().Result)
            {
                using (FileStream output = File.OpenWrite(file))
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