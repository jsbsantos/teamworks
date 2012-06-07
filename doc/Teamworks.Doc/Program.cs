using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Teamworks.Doc
{
    public class Program
    {
        private static string inFolder;
        private static string outFolder;
        private static int willDownload;
        private static List<WaitHandle> handles = new List<WaitHandle>();

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
                    {"<!---T:(.*)-->", m => SimpleReplace(@"\{0}", m)},//tex tag

                    {@"\((.+?)\)[.*]<!---cite-->", m => SimpleReplace(@"\cite{{0}}", m)},//cite tag

                    {"^<!---([a-zA-Z]*)-->", m => SimpleReplace(@"\begin{{0}}[!h]", m)},//begin
                    {"^<!---!([a-zA-Z]*)-->", m => SimpleReplace(@"\end{{0}}", m)},//end

                    {@"(\*\*)(?=\S)(.+?[*]*)(?<=\S)\1", m => SimpleReplace(@"\textbf{{1}}", m)},//bold
                    {@"(\*{1})(?=\S)(.+?[*]*)(?<=\S)\1", m => SimpleReplace(@"\emph{{1}}", m)},//italic

                    {@"^(.*)(?:\s[=])\s$", m => SimpleReplace(@"\section{{0}}", m)},//title
                    {@"^(.*)(?:\s[-])\s$", m => SimpleReplace(@"\subsection{{0}}", m)},//subtitle

                    {@"(?!.*<)^(\*+?\s)", m => SimpleReplace("\\item\n", m)},//simple item
                    {@"(?!.*,)^\*{1}\s*(.*)<!---item-->", m => SimpleReplace("\\item{{0}}\n", m)},//item2
                    {@"(?<=[\.|:|;])[\r\n]{2}", m => SimpleReplace("\\\\ \n", m)},//paragraph
                    {@"!\[(.*)\]\((?<imageuri>.*)\)<!---(?<imagename>.*)-->",
                        #region Download Image Tag    
                        m =>
                        {
                                DownloadImage(m.Groups["imageuri"].Value.Trim(), m.Groups["imagename"].Value.Trim());
                                return SimpleReplace(
                                    @"\includegraphics[width=0.8\textwidth]{images\{2}.png}
\caption{{0}}
\label{{2}}",
                                    m);
                            }
                        #endregion
                        },//image to download
                    {@"(?<!!)\[(.+?)\]{1}\(.*?\)(?!<!---)", m => SimpleReplace(@"\ref{{0}}", m)},//ref
                    {@"(?<!!)\[(.+?)\]{1}\(.*?\)<!---\s*dump\s*-->",
                        #region Reference Dump
                        m =>
                        {
                            DumpToFile(m.Groups[0].ToString());
                            return SimpleReplace(@"\ref{{0}}", m);
                        }
                    #endregion
                        }//reference to dump
                    #endregion
                };


        private static void DownloadImage(string uri, string name)
        {
            if (willDownload == 0)
                return;

            var callback = new Action(
                () =>
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
                        response.ContentType.StartsWith("image",
                                                        StringComparison.OrdinalIgnoreCase))
                    {
                        // if the remote file was found, download it
                        using (Stream inputStream = response.GetResponseStream())
                        {
                            var dir = Path.Combine(outFolder, "images");
                            var file = Path.Combine(dir,
                                                    name +
                                                    uri.Substring(uri.LastIndexOf(".")));

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
                });
            var handle = callback.BeginInvoke(null, null);
            handles.Add(handle.AsyncWaitHandle);
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
            inFolder = args.Length > 0 && !string.IsNullOrEmpty(args[0])
                           ? args[0]
                           : AppDomain.CurrentDomain.BaseDirectory;

            outFolder = args.Length > 1 && !string.IsNullOrEmpty(args[1])
                            ? args[1]
                            : inFolder + @"\pretex\";
            willDownload = args.Length > 2 && !string.IsNullOrEmpty(args[2])
                            ? int.Parse(args[2])
                            : 0;

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

                        File.WriteAllText(
                            Path.Combine(outFolder, new FileInfo(file).Name.Replace(".md", "") + ".pretex"), text,
                            Encoding.UTF8);
                    }
                });

            Console.WriteLine("Waiting for downloads to complete...");
            if (willDownload > 0 && handles.Count > 0)
                WaitHandle.WaitAll(handles.ToArray());
            Console.WriteLine("Press <Enter> to continue...");
            Console.ReadLine();
        }
    }
}