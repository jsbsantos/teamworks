using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.ModelBinding;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Raven.Client.Linq;
using Teamworks.Core.People;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/mailgun")]
    public class MailgunController : RavenApiController
    {
        public HttpResponseMessage Post([ModelBinder(typeof (MailgunModelBinderProvider))]MailgunModel model)
        {
            var person = DbSession.Query<Person>().FirstOrDefault(p => p.Email == model.Sender);
            
            if (person == null)
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            if (!string.IsNullOrEmpty(model.Reply))
            {
                //var thread = DbSession.Query<Thread>()
                //    .Where(t => t.Messages.Any(m => m.Reply == model.Reply))
                //    .SingleOrDefault();

                var message_id = model.Reply.Split(new [] {'.', '@','<'}, StringSplitOptions.RemoveEmptyEntries);

                var thread = DbSession.Load<Thread>(int.Parse(message_id[0]));

                if (thread != null)
                {
                    var message = Message.Forge(model.Message, person.Id);
                    message.Reply = message_id[1];
                    message.Id = thread.GenerateNewTimeEntryId();
                    thread.Messages.Add(message);
                    thread.Notify(message);

                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}