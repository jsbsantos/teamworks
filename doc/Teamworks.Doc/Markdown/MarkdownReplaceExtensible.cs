using System.Linq;
using System.Text.RegularExpressions;

namespace Teamworks.Doc.Markdown
{
    public abstract class MarkdownReplaceExtensible : IMarkdownHandler
    {
        protected MarkdownReplaceExtensible(string template, string pattern)
        {
            Template = template;
            Pattern = pattern;
        }

        public string Template { get; private set; }
        public string Pattern { get; private set; }

        public string Handle(string input)
        {
            return Regex.Replace(input, Pattern,
                                 match =>
                                     {
                                         Extra(match);
                                         var matches = match.Groups.Skip(1).Select(g => g.Value).ToArray();
                                         return Replace(Template, matches);
                                     }, RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        protected abstract void Extra(Match match);
        protected static string Replace(string template, string[] matches)
        {
            var local = template.Substring(0);
            for (var i = 0; i < matches.Length; i++)
            {
                local = local.Replace("{" + i + "}", matches[i].Trim());
            }
            return local;
        }
    }
}