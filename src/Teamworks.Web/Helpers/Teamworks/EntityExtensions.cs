using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Teamworks.Core;

namespace Teamworks.Web.Helpers.Teamworks
{
    public static class EntityExtensions
    {
        public static int Identifier(this string str)
        {
            int i;
            if (string.IsNullOrEmpty(str) || (i = str.IndexOf('/')) < 0)
            {
                return 0;
            }

            int id;
            return int.TryParse(str.Substring(i + 1, str.Length - i - 1), out id) ? id : 0;
        }

        public static string Token(this Entity entity, string user)
        {
            var text = string.Format("{0}:{1}", user, entity.Id);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static void SecureFor(this Person person, string operation)
        {
            
        }
    }
}