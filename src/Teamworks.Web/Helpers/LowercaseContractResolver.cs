using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace Teamworks.Web.Helpers
{
    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            var list = new List<char>();
            for (var i = 0; i < propertyName.Length; i++)
            {
                if (i > 0 && char.IsUpper(propertyName[i]))
                {
                    list.Add('_');
                }
                list.Add(propertyName[i]);
            }
            return new string(list.ToArray()).ToLowerInvariant();
        }
    }
}