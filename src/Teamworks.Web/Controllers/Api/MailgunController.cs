using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Teamworks.Core;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/mailgun")]
    public class MailgunController : RavenApiController
    {
        //[ModelBinder(typeof(MailgunModelBinderProvider))]
        public HttpResponseMessage Post([ModelBinder(typeof(MailgunModelBinderProvider))]Mailgun model)
        {
            var person = DbSession.Query<Core.Person>().FirstOrDefault(p => p.Email == model.Sender);
            
            if (person == null)
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            if (!string.IsNullOrEmpty(model.Reply))
            {
                var messageId = model.Reply.Split(new [] {'.', '@','<'}, StringSplitOptions.RemoveEmptyEntries);

                var discussion = DbSession.Load<Core.Discussion>(int.Parse(messageId[0]));
                
                if (discussion != null)
                {
                    var message = Core.Message.Forge(model.Message, person.Id);
                    message.Reply = messageId[1];
                    message.Id = discussion.GenerateNewTimeEntryId();
                    discussion.Messages.Add(message);
                    discussion.Notify(message);

                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}