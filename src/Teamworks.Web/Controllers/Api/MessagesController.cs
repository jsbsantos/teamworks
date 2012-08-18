using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Extensions.Api;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api/projects/{projectId}/discussions/{discussionId}")]
    public class MessagesController : RavenApiController
    {
        public Discussion GetDiscussion(int discussionId, string entity)
        {
            Discussion discussion = DbSession.Query<Discussion>()
                .FirstOrDefault(d => d.Entity == entity
                                     && d.Id == discussionId.ToId("discussion"));

            Request.NotFound(discussion);
            return discussion;
        }

        [GET("messages")]
        public IEnumerable<MessageViewModel> GetProjectMessages(int projectId, int discussionId)
        {
            IList<Core.Message> messages = GetDiscussion(discussionId, projectId.ToId("project")).Messages;
            return Mapper.Map<IEnumerable<Core.Message>, IEnumerable<MessageViewModel>>(messages);
        }

        [POST("messages")]
        public HttpResponseMessage PostProjectMessages(int projectId, int discussionId, MessageViewModel model)
        {
            Discussion discussion = GetDiscussion(discussionId, projectId.ToId("project"));

            string current = Request.GetCurrentPersonId();
            Core.Message message = Core.Message.Forge(model.Content, current);

            message.Id = discussion.GenerateNewTimeEntryId();

            discussion.Messages.Add(message);
            //discussion.Notify(message, DbSession.Load<Core.Person>(discussion.Subscribers)
            //            .Select(x => x.Email).ToList());

            MessageViewModel value = Mapper.Map<Core.Message, MessageViewModel>(message);
            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        [DELETE("messages/{messageId}")]
        public HttpResponseMessage DeleteProjectMessages(int projectId, int discussionId, int messageId)
        {
            IList<Core.Message> messages = GetDiscussion(discussionId, projectId.ToId("project")).Messages;
            Core.Message message = messages.FirstOrDefault(m => m.Id == messageId);
            Request.NotFound(message);

            messages.Remove(message);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}