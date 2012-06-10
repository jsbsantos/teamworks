using System;

namespace Teamworks.Doc.Markdown
{
    public class ClearPage : IMarkdownHandler
    {
        public string Handle(string input)
        {
            return input + Environment.NewLine + @"\cleardoublepage" + Environment.NewLine;
        }
    }
}