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
        private static void Main(string[] args)
        {
            var folder = args.Length > 0
                         && !string.IsNullOrEmpty(args[0])
                             ? args[0]
                             : AppDomain.CurrentDomain.BaseDirectory;

            var texFolder = Path.Combine(folder, "output", "tex");
            CreateFolder(texFolder);
            
            File.WriteAllBytes(Path.Combine(texFolder, "cover.tex"), Resources.Cover);
            const string format = @"pdflatex -output-directory {0} -interaction=nonstopmode -synctex=1 {1}";

            var batch = Path.Combine(folder, "build.bat");
            File.WriteAllText(batch, @"ECHO OFF" + Environment.NewLine, Encoding.ASCII);


            string name = "rb3007130239.tex";
            var toTex = new MarkdownToTex(name, folder, texFolder, false);
            toTex.RegisterMarkdownHandler(Path.Combine(folder, "output", "images"));

            using (var stream = new StreamWriter(File.Open(batch, FileMode.Append, FileAccess.Write)))
            {
                var output = Console.Out;
                Console.SetOut(stream);
                toTex.CreateTexFileFromMarkdown(texFolder);
                Console.SetOut(output);
            }
            
            using (var stream = new StreamWriter(File.Open(batch, FileMode.Append, FileAccess.Write)))
            {
                stream.WriteLine("COPY {0} {1}", Path.Combine(texFolder, name), Path.Combine(folder, name));
                stream.WriteLine("COPY {0} {1}", Path.Combine(texFolder, "cover.tex"), Path.Combine(folder, "cover.tex"));
                stream.WriteLine("ECHO Now run cover.tex and two times {0}.", name);
                stream.WriteLine("PAUSE");
                stream.Flush();
            }

            var clean = Path.Combine(folder, "clean.bat");
            File.WriteAllText(clean, @"ECHO OFF" + Environment.NewLine, Encoding.ASCII);
            using (
                var stream =
                    new StreamWriter(File.Open(clean, FileMode.Append, FileAccess.Write), Encoding.ASCII))
            {
                stream.WriteLine("ECHO OFF");
                foreach (var directory in Directory.GetDirectories(folder))
                {
                    stream.WriteLine("rd /q/s {0}", directory);
                }
                stream.WriteLine("DEL *.out");
                stream.WriteLine("DEL *.aux");
                stream.WriteLine("DEL *.log");
                stream.WriteLine("DEL *.toc");
                stream.WriteLine("DEL *.synctex.gz");
                stream.WriteLine("DEL *.tex");

                stream.WriteLine("DEL {0}", batch);
                stream.WriteLine("DEL clean.bat");
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