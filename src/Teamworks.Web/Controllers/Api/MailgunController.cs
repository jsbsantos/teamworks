using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.ModelBinding;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Teamworks.Core.People;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/mailgun")]
    public class MailgunController : RavenApiController
    {
        public HttpResponseMessage Post([ModelBinder(typeof (MailgunModelBinderProvider))]MailgunModel model)
        {
            var person = DbSession.Load<Person>().FirstOrDefault(p => p.Email == model.Sender);
            if (person == null)
                return new HttpResponseMessage(HttpStatusCode.NoContent);

            if (!string.IsNullOrEmpty(model.Reply))
            {
                var thread = DbSession.Load<Thread>().FirstOrDefault(t => t.Messages.Select(m => m.Reply).Contains(model.Reply));

                if (thread != null)
                {
                    var message = Message.Forge(model.Message, person.Id);
                    message.Reply = null;
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}