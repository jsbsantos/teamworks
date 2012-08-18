using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Raven.Client.Linq;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Extensions.Api;

namespace Teamworks.Web.Controllers.Api
{
    [SecureProject("projects/view")]
    [RoutePrefix("api/projects/{projectId}")]
    public class DiscussionsController : RavenApiController
    {
        #region ProjectViewModel Discussion

        [GET("discussions")]
        public IEnumerable<Discussion> Get(int projectId)
        {
            IRavenQueryable<Core.Discussion> discussions = DbSession.Query<Core.Discussion>()
                .Where(d => d.Entity == projectId.ToId("project"));

            return Mapper.Map<IEnumerable<Core.Discussion>,
                IEnumerable<Discussion>>(discussions.ToList());
        }

        [GET("discussions/{id}")]
        public Discussion Get(int id, int projectId)
        {
            Core.Discussion discussion = DbSession
                .Query<Core.Discussion>()
                .FirstOrDefault(a => a.Entity == projectId.ToId("project")
                                     && a.Id == id.ToId("discussion"));

            Request.NotFound(discussion);
            return Mapper.Map<Core.Discussion, Discussion>(discussion);
        }

        [POST("discussions")]
        public HttpResponseMessage Post(int projectId, Discussion model)
        {
            var project = DbSession.Load<Project>(projectId);
            Discussion discussion = Discussion.Forge(model.Name, model.Content, project.Id,
                                                               Request.GetCurrentPersonId());

            DbSession.Store(discussion);
            DbSession.SetAuthorizationFor(discussion, new DocumentAuthorization
                                                          {
                                                              Tags = {project.Id}
                                                          });
            Discussion response = Mapper.Map<Core.Discussion, Discussion>(discussion);

            // todo add header of location

            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [PUT("discussions/{id}")]
        public HttpResponseMessage Put(int id, int projectId, Discussion model)
        {
            return null;
        }

        [DELETE("discussions/{id}")]
        public HttpResponseMessage Delete(int id, int projectId)
        {
            Core.Discussion discussion = DbSession
                .Query<Core.Discussion>()
                .FirstOrDefault(a => a.Entity == projectId.ToId("project")
                                     && a.Id == id.ToId("discussion"));

            Request.NotFound(discussion);

            DbSession.Delete(discussion);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion

        [GET("activities/{activityId}/discussions")]
        public IEnumerable<Discussion> GetTaskDiscussions(int projectId, int activityId)
        {
            throw new NotImplementedException();
        }

        [GET("activities/{activityId}/discussions/{id}")]
        public Discussion GetTaskDiscussion(int id, int projectId, int activityId)
        {
            throw new NotImplementedException();
        }

        [POST("activities/{activityId}/discussions/")]
        public HttpResponseMessage PostTaskDiscussion(
            int projectId,
            int activityId,
            Discussion model)
        {
            throw new NotImplementedException();
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("activities/{activityId}/discussions/{id}")]
        public HttpResponseMessage PutTaskDiscussion(int id,
                                                     int projectId,
                                                     int activityId,
                                                     Message model)
        {
            throw new NotImplementedException();
        }

        [DELETE("activities/{activityId}/discussions/{id}")]
        public HttpResponseMessage DeleteTaskDiscussion(int id, int projectId, int activityId)
        {
            throw new NotImplementedException();
        }
    }
}