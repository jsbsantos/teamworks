using System.Text.RegularExpressions;

namespace Teamworks.Doc.Markdown
{
    public class SimpleReplace : SimpleReplaceExtensible
    {
        public SimpleReplace(string template, string pattern) : base(template, pattern)
        {
        }

        protected override void Extra(Match match)
        {
            
        }
    }
}