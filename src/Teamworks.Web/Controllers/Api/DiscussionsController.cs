using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Models;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/discussions")]
    public class DiscussionsController : RavenApiController
    {
        public IEnumerable<DryTopicModel> Get(int projectid)
        {
            var project = DbSession
                .Include<Project>(p => p.Discussions)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return
                new List<DryTopicModel>(
                    DbSession.Load<Topic>(project.Discussions).Select(Mapper.Map<Topic, DryTopicModel>));
        }

        public TopicModel Get(int id, int projectid)
        {
            var topic = DbSession.Load<Topic>(id);
            if (topic == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Mapper.Map<Topic, TopicModel>(topic);
        }

        public HttpResponseMessage<TopicModel> Post([ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                    TopicModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var project = DbSession
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Topic topic = Topic.Forge(model.Name, model.Text, project.Id, Request.GetUserPrincipalId());
            DbSession.Store(topic);
            project.Discussions.Add(topic.Id);
            DbSession.SaveChanges();

            return new HttpResponseMessage<TopicModel>(Mapper.Map<Topic, TopicModel>(topic),
                                                       HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        public HttpResponseMessage Put([ModelBinder(typeof(TypeConverterModelBinder))] int id,
                                       [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                       MessageModel model)
        {
            var project = DbSession
                .Include<Project>(p => p.Discussions)
                .Load<Project>(projectid);
            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var topic = DbSession.Load<Topic>(id);
            if (topic == null || !project.Discussions.Contains(topic.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var message = topic.Messages.FirstOrDefault(m => m.Id.Equals(model.Id));
            if (message == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            message.Date = model.Date;
            message.Text = model.Text;
            DbSession.SaveChanges();

            return new HttpResponseMessage<MessageModel>(Mapper.Map<Message, MessageModel>(message),
                                                         HttpStatusCode.Created);
        }

        public HttpResponseMessage Delete(int id, int projectid)
        {
            var project = DbSession
                .Include<Project>(p => p.Discussions)
                .Load<Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var topic = DbSession.Load<Topic>(id);
            if (topic == null || !project.Discussions.Contains(topic.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            project.Discussions.Remove(topic.Id);
            DbSession.Delete(topic);
            DbSession.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

    }
}
