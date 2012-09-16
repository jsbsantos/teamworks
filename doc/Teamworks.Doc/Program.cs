using System;
using System.Diagnostics;
using System.IO;
using Teamworks.Doc.Markdown;
using Teamworks.Doc.Properties;

namespace Teamworks.Doc
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener(false));
            
            var folder = args.Length > 0
                            && !string.IsNullOrEmpty(args[0])
                                ? args[0]
                                : AppDomain.CurrentDomain.BaseDirectory;

            string output = Path.Combine(folder, "output");
            CreateFolder(output);

            string cover = Path.Combine(output, "cover.tex");
            File.WriteAllBytes(cover, Resources.Cover);
            string signing = Path.Combine(output, "signing.tex");
            File.WriteAllBytes(signing, Resources.Signing);

            string name = "rf3007130239.tex";
            string resume = "r3007130239.tex";
            
            var toTex = new MarkdownToTex();
            toTex.RegisterMarkdownHandler(Path.Combine(folder, "output", "images"));
            toTex.CreateTexFileFromMarkdown(name, folder, output);

            RunProcess("pdflatex",
                       string.Format("-output-directory {0} -interaction=batchmode -synctex=1 {1}", output, cover));
            RunProcess("pdflatex",
                       string.Format("-output-directory {0} -interaction=batchmode -synctex=1 {1}", output, signing));
            RunProcess("pdflatex",
                       string.Format("-output-directory {0} -interaction=batchmode -synctex=1 {1}", output, resume));
            RunProcess("pdflatex",
                       string.Format("-output-directory {0} -interaction=batchmode -synctex=1 {1}", output, name));
            RunProcess("pdflatex",
                       string.Format("-output-directory {0} -interaction=batchmode -synctex=1 {1}", output, name));

            RunProcess("pandoc", string.Format("-s {0} -o {1} ", output + "/" + name + ".pre", name + ".docx"));

            name = name.Replace(".tex", ".pdf");
            resume = resume.Replace(".tex", ".pdf");
            string srcFile = Path.Combine(output, name);
            string srcResume = Path.Combine(output, resume);
            string dstFile = Path.Combine(folder, name);
            string dstResume = Path.Combine(folder, resume);

            if (File.Exists(srcFile))
            {
                if (File.Exists(dstFile))
                {
                    File.Delete(dstFile);
                }
                File.Move(srcFile, dstFile);
            }

            if (File.Exists(srcResume))
            {
                if (File.Exists(dstResume))
                {
                    File.Delete(dstResume);
                }
                File.Move(srcResume, dstResume);
            }


            Directory.Delete(output, true);

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