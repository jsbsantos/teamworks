using System.Collections.Generic;

namespace Teamworks.Web.ViewModels.Api
{
    public class Mailgun : Dictionary<string, string>
    {
        public string Recipient
        {
            get { return this["recipient"]; }
            set { this["recipient"] = value; }
        }

        public string From
        {
            get { return this["from"]; }
            set { this["from"] = value; }
        }

        public string Sender
        {
            get { return this["sender"]; }
            set { this["sender"] = value; }
        }

        public string Subject
        {
            get { return this["subject"]; }
            set { this["subject"] = value; }
        }

        public string Message
        {
            get { return this["stripped-text"]; }
            set { this["stripped-text"] = value; }
        }

        public string Reply
        {
            get { return this["references"] != null ? this["references"].Split(new[] {'\t'})[0] : string.Empty; }
            set { this["references"] = value; }
        }
    }
}