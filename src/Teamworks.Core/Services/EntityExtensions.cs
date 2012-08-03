using Raven.Client.Util;

namespace Teamworks.Core.Services
{
    public static class EntityExtensions
    {
         public static string ToId(this int id, string entity)
         {
             return string.Format("{0}/{1}", Inflector.Pluralize(entity), id);
         }

        public static int ToIdentifier(this string str)
        {
            int i;
            if (string.IsNullOrEmpty(str) || (i = str.IndexOf('/')) < 0)
            {
                return 0;
            }

            int id;
            return int.TryParse(str.Substring(i + 1, str.Length - i - 1), out id) ? id : 0;
        }
    }
}