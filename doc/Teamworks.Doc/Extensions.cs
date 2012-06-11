using System.Collections.Generic;
using System.Text.RegularExpressions;
using Teamworks.Doc.Markdown;

namespace Teamworks.Doc
{
    public static class Extensions
    {
        public static IEnumerable<Group> Skip(this GroupCollection groups, int value)
        {
            var list = new List<Group>();
            for (var i = 0; i < groups.Count; i++)
            {
                if (i < value) continue;
                list.Add(groups[i]);
            }
            return list;
        }

        public static void RegisterMarkdownHandler(this MarkdownToTex md, string folder)
        {
            md.Handlers.Add(new ClearPage());
            md.Handlers.Add(new ImageReplace(folder));
            md.Handlers.Add(new SimpleReplace(@"\_", "(_)"));
            md.Handlers.Add(new SimpleReplace(@"\ref{{0}}", @"\[(.*)\]\(#\)"));
            md.Handlers.Add(new SimpleReplace(@"{0}\cite{{1}}", @"\[([^\]]*)\]*\(#([^)]*)\)"));
            md.Handlers.Add(new SimpleReplace(@"\{0}", "<!---t:(.*)-->"));
            // tex blocks
            md.Handlers.Add(new SimpleReplace(@"\begin{{0}}[!h]", "^<!---([a-zA-Z]*)-->"));
            md.Handlers.Add(new SimpleReplace(@"\end{{0}}", "^<!---!([a-zA-Z]*)-->"));

            md.Handlers.Add(new GlobalReplace("|", "   ", @"\|.*\|"));
        }
    }
}