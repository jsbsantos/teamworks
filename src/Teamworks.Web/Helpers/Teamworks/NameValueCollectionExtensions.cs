using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Teamworks.Core;
using Teamworks.Core.Mailgun;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.Teamworks
{
    public static class NameValueCollectionExtensions
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection source)
        {
            return source.Cast<string>().Select(s => new { Key = s, Value = source[s] }).ToDictionary(p => p.Key, p => p.Value);
        }
    }
}