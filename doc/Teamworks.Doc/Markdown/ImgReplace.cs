using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Teamworks.Doc.Markdown
{
    public class ImgReplace : SimpleReplaceExtensible
    {
        private readonly string _folder;
        private const string _pattern = @"!\[(.*\\label{(?<imagename>.[^}]*)})\]\((?<imageuri>.*/[^.]*(.*))\)";

        
        public ImgReplace(string folder) : base("", _pattern)
        {
            _folder = folder;
            Template = @"![{0}](" + folder.Replace('\\', '/') + @"/{2}{1})";
        }

        protected override void Extra(Match match)
        {
            DownloadImage(match.Groups["imagename"].Value.Trim(), match.Groups["imageuri"].Value.Trim());
        }

        public void DownloadImage(string name, string url)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(!string.IsNullOrEmpty(url));
            
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }

            var client = new HttpClient();
            var result = client.GetAsync(url).Result;
            
            if (!result.IsSuccessStatusCode
                || !result.Content.Headers.ContentType.MediaType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            using (var input = result.Content.ReadAsStreamAsync().Result)
            {
                var i = url.LastIndexOf('.');
                var ext = i > 0 ? url.Substring(i, url.Length - i): ".png";
                
                var file = Path.Combine(_folder, name + ext);
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