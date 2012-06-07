using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Teamworks.Doc
{
    public class Program
    {
        private static string inFolder;
        private static string outFolder;

        private static string SimpleReplace(string template, Match match)
        {
            var group = match.Groups;
            for (int i = 1; i < group.Count; i++)
            {
                template = template.Replace("{" + (i - 1) + "}",
                                            group[i].Value.Trim());
            }
            return template;
        }

        private static Dictionary<string, Func<Match, string>> matchDictionary =
            new Dictionary<string, Func<Match, string>>()
                {
                    #region Config
                    {"^<!---[a-zA-Z]*-->", m => SimpleReplace(@"\begin{{0}}[!h]", m)},
                    {"^<!---![a-zA-Z]*-->", m => SimpleReplace(@"\end{{0}}", m)},
                    {
                        @"!\[(.*)\]\((?<imageuri>.*)\)<!---(?<imagename>.*)-->",
                        m =>
                            {
                                DownloadImage(m.Groups["imageuri"].Value.Trim(), m.Groups["imagename"].Value.Trim());
                                return SimpleReplace(
                                    @"\includegraphics[width=0.8\textwidth]{images\{2}.png}
\caption{{0}}
\label{{2}}",
                                    m);
                            }
                        },
                    {@"(?<!!)\[(.*)\]\(.*\)(?!<!---)", m => SimpleReplace(@"\ref{{0}}", m)},
                    {
                        @"(?<!!)\[(.*)\]\(.*\)<!---dump-->", m =>
                                                                 {
                                                                     DumpToFile(m.Groups[0].ToString());
                                                                     return SimpleReplace(@"\ref{{0}}", m);
                                                                 }
                        }
                    #endregion
                };


        private static void DownloadImage(string uri, string name)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            var response = (HttpWebResponse)request.GetResponse();

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                // if the remote file was found, download it
                using (Stream inputStream = response.GetResponseStream())
                {
                    var dir = Path.Combine(outFolder, "images");
                    var file = Path.Combine(dir, name + uri.Substring(uri.LastIndexOf(".")));

                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    using (Stream outputStream = File.OpenWrite(file))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                }
            }

        }
        private static void DumpToFile(string content)
        {
            using (var stream = File.AppendText(Path.Combine(outFolder, "dump.txt")))
            {
                stream.WriteLine(content);
                stream.Close();
            }
        }

        private static void Main(string[] args)
        {
            inFolder = args.Length > 0 && !string.IsNullOrEmpty(args[0]) ?
                args[0] : AppDomain.CurrentDomain.BaseDirectory;

            outFolder = args.Length > 1 && !string.IsNullOrEmpty(args[1]) ?
                args[1] : inFolder + @"\pretex\";

            if (Directory.Exists(outFolder))
                Directory.Delete(outFolder, true);
            while (Directory.Exists(outFolder)) ;

            Directory.CreateDirectory(outFolder);

            Parallel.ForEach(
                Directory.GetFiles(inFolder, "*.md", SearchOption.TopDirectoryOnly),
                file =>
                {
                    {
                        string text = File.ReadAllText(file);

                        foreach (var keyval in matchDictionary)
                        {
                            Regex regEx = new Regex(keyval.Key, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                            text = regEx.Replace(text, match => keyval.Value.Invoke(match));
                        }

                        File.WriteAllText(Path.Combine(outFolder, new FileInfo(file).Name.Replace(".md", "") + ".pretex"), text,
                                          Encoding.UTF8);
                    }
                });

            Console.WriteLine("Press <Enter> to continue...");
            Console.ReadLine();
        }
    }
}