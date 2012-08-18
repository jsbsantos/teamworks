using System;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Teamworks.Web.Views
{
    public class JsonNetResult : ActionResult
    {
        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings
                                     {
                                         ContractResolver =
                                             new CamelCasePropertyNamesContractResolver(),
                                         NullValueHandling = NullValueHandling.Ignore,
                                         DateTimeZoneHandling = DateTimeZoneHandling.Utc
                                     };
            ContentEncoding = Encoding.UTF8;
            ContentType = "application/json";
            HttpStatusCode = HttpStatusCode.OK;
            HttpStatusDescription = "OK";
        }

        public string HttpStatusDescription { get; set; }

        public object Data { get; set; }
        public string ContentType { get; set; }
        public Encoding ContentEncoding { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.StatusCode = (int) HttpStatusCode;
            response.StatusDescription = HttpStatusDescription;


            response.ContentType = !string.IsNullOrEmpty(ContentType)
                                       ? ContentType
                                       : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output) {Formatting = Formatting};

                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);

                writer.Flush();
            }
        }
    }
}