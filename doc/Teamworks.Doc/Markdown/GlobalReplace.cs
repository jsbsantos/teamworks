using System.Text.RegularExpressions;

namespace Teamworks.Doc.Markdown
{
    public class GlobalReplace : IMarkdownHandler
    {
        private readonly string _dest;
        private readonly string _pattern;
        private readonly string _scr;

        public GlobalReplace(string scr, string dest, string pattern)
        {
            _scr = scr;
            _dest = dest;
            _pattern = pattern;
        }

        #region IMarkdownHandler Members

        public string Handle(string input)
        {
            return Regex.Replace(input, _pattern,
                                 match => match.Groups[0].Value.Replace(_scr, _dest),
                                 RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        #endregion
    }
}