using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Teamworks.Doc.Markdown;
using Teamworks.Doc.Properties;

namespace Teamworks.Doc
{
    public class Program
    {

        #region old
        private static Dictionary<string, Func<Match, string>> matchDictionary =
            new Dictionary<string, Func<Match, string>>()
                {
                    #region Config
                    {"<!---T:(.*)-->", m => SimpleReplace(@"\{0}", m)},
                    //tex tag

                    {
                        @"\((.+?)\)<!---cite-->", m =>
                                                  SimpleReplace(@"\cite{{0}}", m)
                        },
                    //cite tag

                    {@"(\*\*)(?=\S)(.+?[*]*)(?<=\S)\1", m => SimpleReplace(@"\textbf{{1}}", m)},
                    //bold
                    {@"(\*{1})(?=\S)(.+?[*]*)(?<=\S)\1", m => SimpleReplace(@"\emph{{1}}", m)},
                    //italic

                    {@"(?<!!)\[(.+?)\]{1}\(.*?\)(?!<!---)", m => SimpleReplace(@"\ref{{0}}", m)},
                    //ref
                    {
                        @"(?<!!)\[(.+?)\]{1}\(.*?\)<!---\s*dump\s*-->",

                        #region Reference Dump
                        m =>
                            {
                                DumpToFile(m.Groups[0].ToString());
                                return SimpleReplace(@"\ref{{0}}", m);
                            }
                        #endregion
                        },
                    //reference to dump
                    {@"(?!.*>)\|(.*)\|", m => BuildTableRow(m)},
                    {
                        @"^<!---table(?:{((.*?)(?:,(.+?))*)})+-->", m =>
                                                                    SimpleReplace(
                                                                        @"\begin{table}[!ht]
\centering
\begin{tabular}{{1}}",
                                                                        m)
                        },
                    {
                        @"^<!---!table(?:{((.*?)(?:,(.+?))*)})+-->", m =>
                                                                     SimpleReplace(
                                                                         @"\hline
\end{tabular}
\caption{{1}}
\label{{2}}
\end{table}",
                                                                         m)
                        },
                    {"(_)", m => "\\_"}
                    #endregion
                };

        private static string BuildTableRow(Match match)
        {
            return @"\hline" + Environment.NewLine + match.Groups[1].Value.Trim().Replace("|", "&") + "\\\\";
        }

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

      

        private static void DownloadImage(string uri, string name)
        {
            var callback = new Action(
                () =>
                    {
                        var request = (HttpWebRequest) WebRequest.Create(uri);
                        var response = (HttpWebResponse) request.GetResponse();

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
                                var dir = Path.Combine("", "tex/images");
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
            //handles.Add(handle.AsyncWaitHandle);
        }

        private static void DumpToFile(string content)
        {
            using (var stream = File.AppendText(Path.Combine("", "dump.txt")))
            {
                stream.WriteLine(content);
                stream.Close();
            }
        }

        #endregion

        private static void Main(string[] args)
        {
            var folder = args.Length > 0
                         && !string.IsNullOrEmpty(args[0])
                             ? args[0]
                             : AppDomain.CurrentDomain.BaseDirectory;

            var texFolder = Path.Combine(folder, "output");
            var pdfFolder = Path.Combine(folder, "pdf");
            
            CreateFolder(texFolder);
            CreateFolder(pdfFolder);

            File.WriteAllBytes(Path.Combine(texFolder, "cover.tex"), Resources.Cover);
            const string format = @"pdflatex -output-directory {0} -interaction=nonstopmode -synctex=1 {1}";

            var batch = Path.Combine(folder, "build.bat");
            File.WriteAllText(batch, @"echo off" + Environment.NewLine, Encoding.UTF8);
            
            
            string name = "rb3007130239.tex";
            var toTex = new MarkdownToTex(name, folder, texFolder, false);
            toTex.RegisterMarkdownHandler(Path.Combine(folder, "images"));

            using (var stream = new StreamWriter(File.Open(batch, FileMode.Append, FileAccess.Write)))
            {
                var output = Console.Out;
                Console.SetOut(stream);
                toTex.CreateTexFileFromMarkdown(texFolder);
                Console.SetOut(output);
            }
            
            
            using (var stream = new StreamWriter(File.Open(batch, FileMode.Append, FileAccess.Write)))
            {
                var text = string.Format(format, pdfFolder, Path.Combine(texFolder, name));
                
                stream.WriteLine(format, pdfFolder, Path.Combine(texFolder, "cover.tex"));
                stream.WriteLine(text);
                stream.WriteLine(text);
                name = name.Replace(".tex", ".pdf");
                stream.WriteLine("copy {0} {1}", Path.Combine(pdfFolder, name), Path.Combine(folder, name));

                stream.Flush();
            }

            using (var stream = new StreamWriter(File.Open(Path.Combine(folder, "clean.bat"), FileMode.Append, FileAccess.Write)))
            {
                stream.WriteLine("echo off");
                foreach (var directory in Directory.GetDirectories(folder))
                {
                    stream.WriteLine("rd /q/s {0}", directory);
                }
                stream.WriteLine("del {0}", batch);
                stream.WriteLine("del clean.bat");
                stream.Flush();
            }


        }


        private static void CreateFolder(string pdfFolder)
        {
            if (Directory.Exists(pdfFolder))
            {
                Directory.Delete(pdfFolder, true);
            }

            while (Directory.Exists(pdfFolder))
            {
                Thread.SpinWait(1);
            }

            Directory.CreateDirectory(pdfFolder);
        }
    }
}