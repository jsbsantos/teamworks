using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.ModelBinding;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Teamworks.Core;
using Teamworks.Web.Helpers;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api/mailgun")]
    public class MailgunController : RavenApiController
    {
        [POST("teste")]
        public HttpResponseMessage PostAll([ModelBinder(typeof (MailgunModelBinderProvider))] Mailgun model)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        
        [POST("create")]
        public HttpResponseMessage PostCreate([ModelBinder(typeof (MailgunModelBinderProvider))] Mailgun model)
        {
            string str =
                Encoding.UTF8.GetString(Convert.FromBase64String(model.Recipient.Substring(3).Split(new[] {'@'})[0]));
            string[] split = str.Split(new[] {':'});
            string person = split[0];
            string project = split[1];

            Discussion discussion = Discussion.Forge(model.Subject, model.Message, project, person);
            DbSession.Store(discussion);

            var ent = DbSession.Load<Entity>(project);
            var list = ent.GetType().GetProperty("Messages").GetValue(ent, null) as List<string>;
            list.Add(discussion.Id);
            DbSession.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [POST("reply")]
        public HttpResponseMessage PostReply([ModelBinder(typeof(MailgunModelBinderProvider))] Mailgun model)
        {
            Person person = DbSession.Query<Person>().FirstOrDefault(p => p.Email == model.Sender);

            if (person == null)
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            if (!string.IsNullOrEmpty(model.Reply))
            {
                string[] messageId = model.Reply.Split(new[] {'.', '@', '<'}, StringSplitOptions.RemoveEmptyEntries);

                var discussion = DbSession.Load<Discussion>(int.Parse(messageId[0]));

                if (discussion != null)
                {
                    Message message = Message.Forge(model.Message, person.Id);
                    message.Reply = messageId[1];
                    message.Id = discussion.GenerateNewTimeEntryId();
                    discussion.Messages.Add(message);

                    List<string> emails = DbSession.Load<Person>(discussion.Subscribers)
                        .Select(x => x.Email).ToList();

                    discussion.Notify(message, emails);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}