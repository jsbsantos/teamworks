using System.Collections.Generic;
using Teamworks.Core.Projects;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Models
{
    public class MailgunModel : Dictionary<string, string>
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
            get { return this["in-reply-to"]; }
            set { this["in-reply-to"] = value; }
        }
    }

}