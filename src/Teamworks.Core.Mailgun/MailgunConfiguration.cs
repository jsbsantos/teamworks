using System;
using System.Text;

namespace Teamworks.Core.Mailgun
{
    public static class MailgunConfiguration
    {
        public static string Uri
        {
            get { return "https://api.mailgun.net/v2/teamworks.mailgun.org"; }
        }

        private static string _user = "api";

        public static string Username
        {
            get { return _user; }
            set { _user = value; }
        }

        private static string _pwd = "key-20kdn4zyszbkxvk63baq02sxppv2a4n5";

        public static string Password
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        public static string Credentials
        {
            get
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                    throw new ArgumentException("Username or password are null or empty.");

                return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", Username, Password)));
            }
        }

        private static string _host = @"Teamworks <notifications@teamworks.mailgun.org>";
        public static string Host
        {
            get { return _host; }
            set { _host = value; }
        }
    }
}