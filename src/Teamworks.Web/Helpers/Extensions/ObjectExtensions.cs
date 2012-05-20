using Newtonsoft.Json;

namespace Teamworks.Web.Helpers.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None,
                                               new JsonSerializerSettings()
                                                   {ContractResolver = new JsonNetFormatter.LowercaseContractResolver()});
        }
    }
}