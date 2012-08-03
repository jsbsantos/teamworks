using System;
using System.Text;
using Teamworks.Core;

namespace Teamworks.Web.Helpers.App
{
    public static class EntityExtensions
    {
        public static string Token(this Entity entity, string user)
        {
            var text = string.Format("{0}:{1}", user, entity.Id);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }
    }
}