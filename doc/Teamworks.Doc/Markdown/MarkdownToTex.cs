using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Teamworks.Doc.Properties;

namespace Teamworks.Doc.Markdown
{
    public class MarkdownToTex
    {
        public MarkdownToTex()
        {
            Handlers = new List<IMarkdownHandler>();
        }

        public List<IMarkdownHandler> Handlers { get; set; }

        public void CreateTexFileFromMarkdown(string name, string input, string output)
        {
            var pre = Path.Combine(output, name + ".pre");
            
            File.WriteAllBytes(Path.Combine(output, "template.latex"), Resources.Template);

            var c = 0;
            var appendix = false;
            string content = null;
            var index = Path.Combine(input, "index.md");
            File.WriteAllText(pre, @"<!--- automatic -->" + Environment.NewLine + Environment.NewLine, Encoding.UTF8);
            using (var stream = File.OpenText(index))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    c++;

                    if (Regex.IsMatch(line, @"^###\s"))
                    {
                        if (appendix)
                        {
                            Trace.WriteLine(String.Format("I[{0}]: {1}", c, line));
                            continue;
                        }

                        var file = Regex.Match(line, @"[^[]\[.*\]\([^)]*/(.*)\)",
                                               RegexOptions.Multiline | RegexOptions.IgnoreCase)
                            .Groups[1].Value;

                        content = File.ReadAllText(Path.Combine(input, file));
                    }
                    else if (Regex.IsMatch(line, @"^####\s"))
                    {
                        if (!appendix)
                        {
                            File.AppendAllText(pre, @"\appendix\def\thesection{\Alph{section}}" + Environment.NewLine);
                            appendix = true;    
                        }
                        
                        var file = Regex.Match(line, @"[^[]\[.*\]\([^)]*/(.*)\)",
                                               RegexOptions.Multiline | RegexOptions.IgnoreCase)
                            .Groups[1].Value;

                        content = File.ReadAllText(Path.Combine(input, file));
                    } else
                    {
                        Trace.WriteLine(String.Format("I[{0}]: {1}", c, line));
                        continue;
                    }

                    if (String.IsNullOrEmpty(content)) continue;

                    var result = MarkdownHandlersPipeline(content, Handlers);
                    File.AppendAllText(pre, Environment.NewLine + result, Encoding.UTF8);
                }
            }

            var front = Path.Combine(input, "front.tex");
            var exists = File.Exists(front);
            var args = String.Format(
            @"--variable=lang:portuguese --variable=fontssize:11pt --variable=linkcolor:black --variable=tables:true --variable=graphics:true --from=markdown --to=latex --output={0} --listings --standalone --template={1}  --number-sections {2} --toc {3}",
            Path.Combine(output, name), Path.Combine(output, "template.latex"), exists ? "--include-before=" + front: "", pre);

            Trace.WriteLine("pandoc " + args);
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "pandoc",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };
            process.Start();

            process.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);
            process.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
            process.WaitForExit();
        }


        private static string MarkdownHandlersPipeline(string input, IEnumerable<IMarkdownHandler> handlers)
        {
            return handlers.Aggregate(input, (current, handler) => handler.Handle(current));
        }
    }
}