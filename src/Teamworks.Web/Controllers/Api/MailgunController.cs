using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Teamworks.Core;
using Teamworks.Web.Helpers;
using Teamworks.Web.Models;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/mailgun")]
    public class MailgunController : RavenApiController
    {
        [POST("teste")]
        public HttpResponseMessage PostAll([ModelBinder(typeof(MailgunModelBinderProvider))] Mailgun model)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST("create")]
        public HttpResponseMessage PostCreate([ModelBinder(typeof (MailgunModelBinderProvider))] Mailgun model)
        {
            var str =
                Encoding.UTF8.GetString(Convert.FromBase64String(model.Recipient.Substring(3).Split(new char[] {'@'})[0]));
            var split = str.Split(new char[] {':'});
            var person = split[0];
            var project = split[1];

            var discussion = Core.Discussion.Forge(model.Subject, model.Message, project, person);
            DbSession.Store(discussion);

            var ent = DbSession.Load<Entity>(project);
            var list = ent.GetType().GetProperty("Messages").GetValue(ent,null) as List<string>;
            list.Add(discussion.Id);
            DbSession.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST("reply")]
        public HttpResponseMessage PostReply([ModelBinder(typeof (MailgunModelBinderProvider))] Mailgun model)
        {
            var person = DbSession.Query<Core.Person>().FirstOrDefault(p => p.Email == model.Sender);

            if (person == null)
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            if (!string.IsNullOrEmpty(model.Reply))
            {
                var messageId = model.Reply.Split(new[] {'.', '@', '<'}, StringSplitOptions.RemoveEmptyEntries);

                var discussion = DbSession.Load<Core.Discussion>(int.Parse(messageId[0]));

                if (discussion != null)
                {
                    var message = Core.Message.Forge(model.Message, person.Id);
                    message.Reply = messageId[1];
                    message.Id = discussion.GenerateNewTimeEntryId();
                    discussion.Messages.Add(message);

                    var emails = DbSession.Load<Core.Person>(discussion.Subscribers)
                        .Select(x => x.Email).ToList();

                    discussion.Notify(message, emails);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}