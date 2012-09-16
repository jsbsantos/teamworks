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
            var front = Path.Combine(output, "front.tex");
            var pre = Path.Combine(output, name + ".pre");
            
            File.WriteAllBytes(Path.Combine(output, "template.latex"), Resources.Template);
            File.WriteAllText(pre, @"<!--- automatic -->" + Environment.NewLine + Environment.NewLine, Encoding.UTF8);

            var c = 0;
            var before = true;
            var index = Path.Combine(input, "index.md");
            using (var stream = File.OpenText(index))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    c++;
                    Trace.WriteLine(String.Format("I[{0}]: {1}", c, line));
                    if (Regex.IsMatch(line, @"[ ]*<!---[ ]*appendix[ ]*-->"))
                    {
                        File.AppendAllText(pre, @"\appendix\def\thesection{\Alph{section}}" + Environment.NewLine);
                    }
                    else if (Regex.IsMatch(line, @"[ ]*<!---[ ]*main[ ]*-->[ ]*"))
                    {
                        before = false;
                    }
                    else if (Regex.IsMatch(line, @"^###\s"))
                    {
                        if (before)
                        {
                            var file = Regex.Match(line, @"[^[]\[.*\]\([^)]*/(.*)\)",
                                                   RegexOptions.Multiline | RegexOptions.IgnoreCase)
                                .Groups[1].Value;
                            var content = File.ReadAllText(Path.Combine(input, file));
                            if (String.IsNullOrEmpty(content)) continue;

                            var result = MarkdownHandlersPipeline(content, Handlers);
                            File.AppendAllText(front, Environment.NewLine + result, Encoding.UTF8);
                        }
                        else
                        {
                            var file = Regex.Match(line, @"[^[]\[.*\]\([^)]*/(.*)\)",
                                                   RegexOptions.Multiline | RegexOptions.IgnoreCase)
                                .Groups[1].Value;

                            var content = File.ReadAllText(Path.Combine(input, file));
                            if (String.IsNullOrEmpty(content)) continue;

                            var result = MarkdownHandlersPipeline(content, Handlers);
                            File.AppendAllText(pre, Environment.NewLine + result, Encoding.UTF8);
                        }
                    }
                }
            }

            var bib = string.Empty;
            foreach (var f in Directory.GetFiles(input, "*.bib", SearchOption.TopDirectoryOnly))
            {
                var file = Path.Combine(input, f);
                bib += " --bibliography=" + file;
            }

            var exists = File.Exists(front);
            var args = String.Format(
                @"--variable=lang:portuguese --variable=fontsize:12pt --variable=linkcolor:black --variable=tables:true --variable=graphics:true --from=markdown --to=latex --output={0} --listings --standalone --template={1}  --number-sections {2} --toc {3} {4}",
                Path.Combine(output, name), Path.Combine(output, "template.latex"), bib,
                exists ? "--include-before=" + front : "", pre);

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

            File.WriteAllText(Path.Combine(output, "r3007130239.pre"), "", Encoding.UTF8);
            File.WriteAllBytes(Path.Combine(output, "resume.latex"), Resources.Resume);

            if (!exists) return;
            args = String.Format(
                @"--variable=lang:portuguese --variable=fontsize:12pt --variable=linkcolor:black --variable=tables:true --variable=graphics:true --from=markdown --to=latex --output={0} --listings --standalone --template={1} {2}",
                Path.Combine(output, "r3007130239.tex"), Path.Combine(output, "resume.latex"), front);

            Trace.WriteLine("pandoc " + args);
            process = new Process
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