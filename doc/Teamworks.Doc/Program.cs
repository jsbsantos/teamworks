using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RavenDBConsole
{
    public class Program
    {

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
                    {"^<!---[a-zA-Z]*-->", m => SimpleReplace(@"\begin{{0}}[!h]",m)},
                    {"^<!---![a-zA-Z]*-->", m => SimpleReplace(@"\end{{0}}",m)},
                    {
                        @"!\[(.*)\]\((?<imageuri>.*)\)<!---(?<imagename>.*)-->",
                        m =>
                            {

                                DownloadImage(m.Groups["imageuri"].Value, m.Groups["imagename"].Value);
                                return SimpleReplace(
                                    @"\includegraphics[width=0.8\textwidth]{images\{2}.png}
\caption{{0}}
\label{{2}}",
                                    m);

                            }
                        },
                        {@"(?<!!)\[(.*)\]\(.*\)(?!<!---)", m => SimpleReplace(@"\ref{{0}}",m)},
                        {@"(?<!!)\[(.*)\]\(.*\)<!---dump-->", m =>
                                                                {
                                                                    Console.WriteLine("DUMP:"+ m.Groups[0]);
                                                                    return SimpleReplace(@"\ref{{0}}", m);
                                                                }}
                };

        private static void DownloadImage(string uri, string name)
        {
            Console.WriteLine("Name: " + name + ", Uri: " + uri);
        }

        private static void Main(string[] args)
        {
            var inFolder = @"D:\MyDocuments\GitHub\LI61N-G07\doc\rb";
            var outFolder = inFolder + @"\pretex\";
            if (Directory.Exists(outFolder))
                Directory.Delete(outFolder, true);

            var x = Directory.CreateDirectory(outFolder);
            while (!x.Exists)
            {
            }

            Parallel.ForEach(
                Directory.GetFiles(inFolder, "*.md", SearchOption.TopDirectoryOnly),
                (file) =>
                {
                    {
                        string text = File.ReadAllText(file);

                        foreach (var keyval in matchDictionary)
                        {
                            Regex regEx = new Regex(keyval.Key, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                            text = regEx.Replace(text, match => keyval.Value.Invoke(match));
                        }

                        File.WriteAllText(outFolder + new FileInfo(file).Name.Replace(".md", "") + ".pretex", text,
                                          Encoding.UTF8);
                    }
                });

            Console.ReadLine();
        }
    }
}