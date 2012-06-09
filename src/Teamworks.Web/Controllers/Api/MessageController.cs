using System;
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

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/projects/{projectid}/discussions/{discussionid}/messages")]
    public class MessageController : RavenApiController
    {
        private Topic LoadDiscussion(int projectid, int discussionid)
        {
            var project = DbSession
                .Include<Project>(p => p.Discussions)
                .Load<Project>(projectid);
            if (project == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var topic = DbSession.Load<Topic>(discussionid);
            if (topic == null || !project.Discussions.Contains(topic.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return topic;
        }

        public IEnumerable<MessageModel> Get(int projectid, int discussionid)
        {
            return LoadDiscussion(projectid, discussionid).Messages.Select(Mapper.Map<Message, MessageModel>);
        }

        public HttpResponseMessage<MessageModel> Post([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                      [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                      MessageModel model)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var topic = LoadDiscussion(projectid, discussionid);

            var message = Message.Forge(model.Text, Request.GetUserPrincipalId());
            message.Id = topic.GenerateNewTimeEntryId();
            topic.Messages.Add(message);
            DbSession.SaveChanges();

            return new HttpResponseMessage<MessageModel>(Mapper.Map<Message, MessageModel>(message),
                                                         HttpStatusCode.Created);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        public HttpResponseMessage<MessageModel> Put([ModelBinder(typeof(TypeConverterModelBinder))] int discussionid,
                                                     [ModelBinder(typeof(TypeConverterModelBinder))] int projectid,
                                                     MessageModel model)
        {
            throw new NotImplementedException();
            /* if (!ModelState.IsValid)
             {
                 throw new HttpResponseException(HttpStatusCode.BadRequest);
             }

             var topic = LoadDiscussion(projectid, discussionid);

             var message = topic.Messages.FirstOrDefault(t => t.Id.Equals(model.Id));
             if (message == null)
             {
                 throw new HttpResponseException(HttpStatusCode.NotFound);
             }

             message.Text = model.Text;
             DbSession.SaveChanges();

             return new HttpResponseMessage<MessageModel>(Mapper.Map<Message, MessageModel>(message),
                                                          HttpStatusCode.Created);*/
        }

        public HttpResponseMessage Delete(int id, int projectid, int discussionid)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var topic = LoadDiscussion(projectid, discussionid);

            var message = topic.Messages.FirstOrDefault(m => m.Id == id);
            if (message == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            topic.Messages.Remove(message);
            DbSession.SaveChanges();

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

    }
}