﻿using System;
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

            var cover = Path.Combine(output, "cover.tex");
            File.WriteAllBytes(cover, Resources.Cover);
            
            var name = "rb3007130239.tex";
            var toTex = new MarkdownToTex();
            toTex.RegisterMarkdownHandler(Path.Combine(folder, "output", "images"));
            toTex.CreateTexFileFromMarkdown(name, folder, output);

            RunProcess("pdflatex", string.Format("-output-directory {0} -interaction=batchmode -synctex=1 {1}", output, cover));
            RunProcess("pdflatex", string.Format("-output-directory {0} -interaction=batchmode -synctex=1 {1}", output, name));
            RunProcess("pdflatex", string.Format("-output-directory {0} -interaction=batchmode -synctex=1 {1}", output, name));

            name = name.Replace(".tex", ".pdf");
            var srcFile = Path.Combine(output, name);
            var dstFile = Path.Combine(folder, name);

            if (File.Exists(srcFile))
            {
                if (File.Exists(dstFile))
                {
                    File.Delete(dstFile);
                }
                File.Move(srcFile, dstFile);
            }
                

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