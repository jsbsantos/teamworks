using System.Text.RegularExpressions;
using Teamworks.Doc.Markdown;

namespace Teamworks.Doc
{
    public class AppendToEnd : IMarkdownHandler
    {
        private readonly string _str;
        private readonly string _pattern;

        public AppendToEnd(string str, string pattern)
        {
            _str = str;
            _pattern = pattern;
        }

        public string Handle(string input)
        {
            return Regex.Replace(input, _pattern,
                                 match => match.Groups[0] + _str,
                                 RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }
    }
}