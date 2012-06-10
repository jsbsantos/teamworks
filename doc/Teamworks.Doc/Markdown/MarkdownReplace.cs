using System.Text.RegularExpressions;

namespace Teamworks.Doc.Markdown
{
    public class MarkdownReplace : MarkdownReplaceExtensible
    {
        public MarkdownReplace(string template, string pattern) : base(template, pattern)
        {
        }

        protected override void Extra(Match match)
        {
            
        }
    }
}