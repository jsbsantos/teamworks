﻿using System.Collections.Generic;

namespace Teamworks.Core.Mailgun
{
    public class MailgunMessages : Dictionary<string, string>
    {
        public string From
        {
            get { return this["from"]; }
            set { this["from"] = value; }
        }

        public string To
        {
            get { return this["to"]; }
            set { this["to"] = value; }
        }

        public string Subject
        {
            get { return this["subject"]; }
            set { this["subject"] = value; }
        }

        public string Cc
        {
            get { return this["cc"]; }
            set { this["cc"] = value; }
        }

        public string Bcc
        {
            get { return this["bcc"]; }
            set { this["bcc"] = value; }
        }

        public string Message
        {
            get { return this["text"]; }
            set { this["text"] = value; }
        }

        public string Id { get; internal set; }
    }
}