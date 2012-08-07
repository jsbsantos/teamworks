using System.Linq;
using System.Text.RegularExpressions;

namespace Teamworks.Doc.Markdown
{
    public abstract class SimpleReplaceExtensible : IMarkdownHandler
    {
        protected SimpleReplaceExtensible(string template, string pattern)
        {
            Template = template;
            Pattern = pattern;
        }

        public string Template { get; protected set; }
        public string Pattern { get; private set; }

        #region IMarkdownHandler Members

        public string Handle(string input)
        {
            return Regex.Replace(input, Pattern,
                                 match =>
                                     {
                                         Extra(match);
                                         string[] matches = match.Groups.Skip(1).Select(g => g.Value).ToArray();
                                         return Replace(Template, matches);
                                     }, RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        #endregion

        protected abstract void Extra(Match match);

        protected static string Replace(string template, string[] matches)
        {
            string local = template.Substring(0);
            for (int i = 0; i < matches.Length; i++)
            {
                local = local.Replace("{" + i + "}", matches[i].Trim());
            }
            return local;
        }
    }
}