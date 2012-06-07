using Newtonsoft.Json;
using Raven.Client.Authorization;
using Teamworks.Core.People;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.Teamworks
{
    public static class EntityExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None,
                                               new JsonSerializerSettings() { ContractResolver = new JsonNetFormatter.LowercaseContractResolver() });
        }

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

        public static void SecureFor(this Person person, string operation)
        {
            
        }
    }
}