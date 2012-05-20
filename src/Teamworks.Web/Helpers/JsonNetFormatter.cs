using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Teamworks.Web.Helpers
{
    /// <see cref="http://blogs.msdn.com/b/henrikn/archive/2012/02/18/using-json-net-with-asp-net-web-api.aspx" />
    public class JsonNetFormatter : MediaTypeFormatter
    {
        public JsonNetFormatter()
            : this(null)
        {
        }

        public JsonNetFormatter(JsonSerializerSettings jsonSerializerSettings)
        {
            JsonSerializerSettings = jsonSerializerSettings ??
                                     new JsonSerializerSettings {ContractResolver = new LowercaseContractResolver()};

            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            Encoding = new UTF8Encoding(false, true);
        }

        private JsonSerializerSettings JsonSerializerSettings { get; set; }

        protected override bool CanReadType(Type type)
        {
            return type != typeof (IKeyValueModel);
        }

        protected override bool CanWriteType(Type type)
        {
            return true;
        }

        protected override Task<object> OnReadFromStreamAsync(Type type, Stream stream,
                                                              HttpContentHeaders contentHeaders,
                                                              FormatterContext formatterContext)
        {
            JsonSerializer serializer = JsonSerializer.Create(JsonSerializerSettings);
            return Task.Factory.StartNew(() =>
                                             {
                                                 using (var streamReader = new StreamReader(stream, Encoding))
                                                 {
                                                     using (
                                                         var jsonTextReader = new JsonTextReader(streamReader)
                                                         )
                                                     {
                                                         return serializer.Deserialize(jsonTextReader, type);
                                                     }
                                                 }
                                             });
        }

        protected override Task OnWriteToStreamAsync(Type type, object value, Stream stream,
                                                     HttpContentHeaders contentHeaders,
                                                     FormatterContext formatterContext,
                                                     TransportContext transportContext)
        {
            JsonSerializer serializer = JsonSerializer.Create(JsonSerializerSettings);

            return Task.Factory.StartNew(() =>
                                             {
                                                 using (
                                                     var jsonTextWriter =
                                                         new JsonTextWriter(new StreamWriter(stream, Encoding))
                                                             {CloseOutput = false})
                                                 {
                                                     serializer.Serialize(jsonTextWriter, value);
                                                     jsonTextWriter.Flush();
                                                 }
                                             });
        }

        #region Nested type: LowercaseContractResolver

        /// <see cref="http://nyqui.st/json-net-newtonsoft-json-lowercase-keys" />
        public class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }

        #endregion
    }
}