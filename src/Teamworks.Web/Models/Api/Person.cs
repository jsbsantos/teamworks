using System;
using System.Security.Cryptography;
using System.Text;

namespace Teamworks.Web.Models.Api
{
    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        private string _email;

        public string Email { get; set; }
        public string Gravatar { get; set; }

        public static string GravatarUrl(string email)
        {
            const string baseUrl = "http://www.gravatar.com/avatar/";

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(email.Trim()))
                throw new ArgumentException("The email is empty.", "email");

            var md5 = MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(email.ToLower()));
            var sb = new StringBuilder(baseUrl);
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            sb.Append("?r=g");
            return sb.ToString();
        }
    }
}