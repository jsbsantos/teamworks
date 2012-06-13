using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Teamworks.Doc.Markdown;
using Teamworks.Doc.Properties;

namespace Teamworks.Doc
{
    public static class Program
    {

        private static void Main(string[] args)
        {
            Encoding encoding = new UTF8Encoding(false);

            Trace.Listeners.Add(new ConsoleTraceListener(false));
            var folder = args.Length > 0
                         && !string.IsNullOrEmpty(args[0])
                             ? args[0]
                             : AppDomain.CurrentDomain.BaseDirectory;

            var output = Path.Combine(folder, "output");
            CreateFolder(output);
            
            File.WriteAllBytes(Path.Combine(folder, "cover.tex"), Resources.Cover);
            
            const string name = "rb3007130239.tex";
            var toTex = new MarkdownToTex();
            toTex.RegisterMarkdownHandler(Path.Combine(folder, "output", "images"));
            toTex.CreateTexFileFromMarkdown(name, folder, output);

            var dest = Path.Combine(folder, name);
            if (File.Exists(dest))
            {
                File.Delete(dest);
            }

            File.Move(Path.Combine(output, name), dest);

            var clean = Path.Combine(folder, "clean.bat");
            File.WriteAllText(clean, @"ECHO OFF" + Environment.NewLine, encoding);
            
            using (var stream = new StreamWriter(
                File.Open(clean, FileMode.Append, FileAccess.Write), encoding))
            {
                stream.WriteLine("ECHO OFF");
                foreach (var directory in Directory.GetDirectories(folder))
                {
                    stream.WriteLine("RD /q/s {0}", directory);
                }
                stream.WriteLine("DEL *.out");
                stream.WriteLine("DEL *.aux");
                stream.WriteLine("DEL *.log");
                stream.WriteLine("DEL *.toc");
                stream.WriteLine("DEL *.synctex.gz");
                stream.WriteLine("DEL *.tex");

                stream.WriteLine("DEL clean.bat");
                stream.Flush();
            }

            RunProcess("pdflatex", @"-output-directory ./output/ -interaction=batchmode -synctex=1 cover.tex");
            RunProcess("pdflatex", @"-output-directory ./output/ -interaction=batchmode -synctex=1 rb3007130239.tex");
            RunProcess("pdflatex", @"-output-directory ./output/ -interaction=batchmode -synctex=1 rb3007130239.tex");

            var srcFile = Path.Combine(output, "rb3007130239.pdf");
            var dstcFile = Path.Combine(folder, "rb3007130239.pdf");

            if (File.Exists(srcFile))
                File.Move(srcFile, dstcFile);

            Directory.Delete(output,true);

            Console.WriteLine(@"Press <enter> to end process.");
            Console.ReadLine();
        }


        private static void CreateFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        private static void RunProcess(string processName, string args)
        {
            Trace.WriteLine(processName + " " + args);
            var process = new Process
            {
                StartInfo =
                {
                    FileName = processName,
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
    }
}