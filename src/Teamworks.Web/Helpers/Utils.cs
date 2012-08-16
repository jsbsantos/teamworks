using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Teamworks.Web.Helpers
{
    public static class Utils
    {
        public static JsonSerializerSettings JsonSettings()
        {
            return new JsonSerializerSettings
                       {
                           ContractResolver = new CamelCasePropertyNamesContractResolver(),
                           NullValueHandling = NullValueHandling.Ignore
                       };
        }

        public static string Hash(string str)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(
                Encoding.Default.GetBytes(str));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string ToJson(object model)
        {
            return JsonConvert.SerializeObject(model,
                                               new JsonSerializerSettings
                                                   {
                                                       ContractResolver =
                                                           new CamelCasePropertyNamesContractResolver(),
                                                       NullValueHandling = NullValueHandling.Ignore,
                                                       DateTimeZoneHandling = DateTimeZoneHandling.Utc
                                                   });
        }

        public static string ToIndentedJson(object model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented,
                                               new JsonSerializerSettings
                                                   {
                                                       ContractResolver =
                                                           new CamelCasePropertyNamesContractResolver(),
                                                       NullValueHandling = NullValueHandling.Ignore,
                                                       DateTimeZoneHandling = DateTimeZoneHandling.Utc
                                                   });
        }
    }
}