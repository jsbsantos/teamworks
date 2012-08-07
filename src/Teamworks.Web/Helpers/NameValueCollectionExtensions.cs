using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Teamworks.Web.Helpers
{
    public static class NameValueCollectionExtensions
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection source)
        {
            return source.Cast<string>().Select(s =>
                                                new {Key = s, Value = source.GetValues(s)[0]})
                .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}