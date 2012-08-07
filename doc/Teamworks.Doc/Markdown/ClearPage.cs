using System;

namespace Teamworks.Doc.Markdown
{
    public class ClearPage : IMarkdownHandler
    {
        #region IMarkdownHandler Members

        public string Handle(string input)
        {
            return input + Environment.NewLine + @"\cleardoublepage" + Environment.NewLine;
        }

        #endregion
    }
}