using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Teamworks.Doc.Properties;

namespace Teamworks.Doc.Markdown
{
    public class MarkdownToTex
    {
        private readonly string _name;
        private readonly string _input;
        private readonly string _batch;
        private readonly string _output;
        private readonly string _temporary;
        private readonly bool _recursive;
        
        public MarkdownToTex(string name, string input, bool recursive) : this (name, input, null, recursive)
        {
            
        }

        public MarkdownToTex(string name, string input, string output, bool recursive)
        {
            _name = name;
            _input = input;
            _temporary = Path.Combine(_input, "output", "pretex");
            _output = output ?? Path.Combine(_input, "output", "tex");
            _batch = Path.Combine(_input, "build.bat");
            _recursive = recursive;
            Handlers = new List<IMarkdownHandler>();
        }

        public List<IMarkdownHandler> Handlers { get; set; }
        public void CreateTexFileFromMarkdown(string folder)
        {
            if (Directory.Exists(_temporary))
            {
                Directory.Delete(_temporary, true);
            }

            while (Directory.Exists(_temporary))
            {
                Thread.SpinWait(1);
            }

            Directory.CreateDirectory(_temporary);
            
            foreach (var file in Directory.GetFiles(_input, "*.md", _recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                if (file.EndsWith("index.md", StringComparison.OrdinalIgnoreCase)) continue;
                var result = MarkdownHandlersPipeline(File.ReadAllText(file), Handlers);
                var name = new FileInfo(file).Name.Replace(".md", ".pretex");
                File.WriteAllText(Path.Combine(_temporary, name), result, Encoding.UTF8);
            }

            BaseFiles(_output);
            TexFiles();
            Console.WriteLine(@"@copy NUL blank.md");
            Finish();
            Console.WriteLine(@"@del blank.md");
            
        }

        #region Helpers

        private void TexFiles()
        {
            foreach (var file in Directory.GetFiles(_temporary, "*.pretex", SearchOption.TopDirectoryOnly))
            {
                var info = new FileInfo(file);
                var input = info.FullName;
                var output = Path.Combine(_output, info.Name.Replace(".pretex", ".tex"));
                var line = string.Format("@pandoc --from=markdown --to=latex --output={0} {1}", output, input);
                Console.WriteLine(line);
            }
        }

        private static void BaseFiles(string output)
        {
            File.WriteAllBytes(Path.Combine(output, "front.tex"), Resources.Front);
            File.WriteAllBytes(Path.Combine(output, "header.tex"), Resources.Header);
            File.WriteAllBytes(Path.Combine(output, "template.latex"), Resources.Template);
        }
        
        private void Finish()
        {
            var files = FilesToInclude(File.ReadAllText(Path.Combine(_input, "index.md")));
            var include = new StringBuilder();
            foreach (var file in files)
            {
                include.Append(string.Format(" --include-after-body=\"{0}\"", Path.Combine(_output, file.Replace(".md", ".tex"))));
            }

            var line =
                string.Format(
                    @"@pandoc --variable lang=portuguese --variable linkcolor=black --variable tables=true --variable graphics=true --from=markdown --to=latex --output={0} --listings --include-in-header={1}\header.tex --standalone --template={1}\template.latex  --number-sections --include-before-body={1}\front.tex --toc {2} .\blank.md",
                    Path.Combine(_output, _name), _output, include);
            Console.WriteLine(line);
        }

        private static IEnumerable<string> FilesToInclude(string source)
        {
            var list = new List<string>();
            var matches = Regex.Matches(source, @"\s###\s\[(.+?)\]{1}\((?<source>.*?)\)");
            for (var m = 0; m < matches.Count; m++)
            {
                var match = matches[m];
                var file = match.Groups["source"].Value;
                var i = file.LastIndexOf('/') + 1;
                var name = i > 0 ? file.Substring(i, file.Length - i) : file;
                if (File.Exists(name))
                {
                    list.Add(name);
                }
            }
            return list.ToArray();
        }

        private static string MarkdownHandlersPipeline(string input, IEnumerable<IMarkdownHandler> handlers )
        {
            return handlers.Aggregate(input, (current, handler) => handler.Handle(current));
        }

        #endregion
    }
}