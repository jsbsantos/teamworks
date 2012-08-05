using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Teamworks.Web.Helpers
{
    public static class Utils
    {
        public static JsonSerializerSettings JsonSettings()
        {
            return new JsonSerializerSettings {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };
        }
    }
}