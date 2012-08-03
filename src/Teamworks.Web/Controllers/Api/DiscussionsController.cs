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
using Teamworks.Core.Services;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Controllers.Api
{
    [SecureFor]
    [RoutePrefix("api/projects/{projectId}")]
    public class DiscussionsController : RavenApiController
    {
        #region Project Discussion

        [GET("discussions")]
        public IEnumerable<Discussion> Get(int projectId)
        {
            var discussions = DbSession.Query<Core.Discussion>()
                .Where(d => d.Entity == projectId.ToId("project"));
            
            return Mapper.Map<IEnumerable<Core.Discussion>, 
                IEnumerable<Discussion>>(discussions.ToList());
        }

        [GET("discussions/{id}")]
        public Discussion Get(int id, int projectId)
        {
            var discussion = DbSession
                .Query<Core.Discussion>()
                .FirstOrDefault(a => a.Entity == projectId.ToId("project")
                                     && a.Id == id.ToId("discussion"));
            
            Request.ThrowNotFoundIfNull(discussion);
            return Mapper.Map<Core.Discussion, Discussion>(discussion);
        }

        [POST("discussions")]
        public HttpResponseMessage Post(int projectId, Discussion model)
        {
            var project = DbSession.Load<Core.Project>(projectId);
            var discussion = Core.Discussion.Forge(model.Name, model.Content, project.Id, Request.GetCurrentPersonId());

            DbSession.Store(discussion);
            DbSession.SetAuthorizationFor(discussion, new DocumentAuthorization
                                                          {
                                                              Tags = {project.Id}
                                                          });


            var response = Mapper.Map<Core.Discussion, Discussion>(discussion);

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
            var discussion = DbSession
                .Query<Core.Discussion>()
                .FirstOrDefault(a => a.Entity == projectId.ToId("project")
                                     && a.Id == id.ToId("discussion"));

            Request.ThrowNotFoundIfNull(discussion);
            
            DbSession.Delete(discussion);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion

        // todo
        #region Activities Discussion

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

        #region Person

        [GET("activities/{activityId}/discussions/{id}/people")]
        public IEnumerable<Person> GetPeople_Activity(int id, int projectId)
        {
            throw new NotImplementedException();
        }

        [POST("activities/{activityId}/discussions/{id}/people")]
        public HttpResponseMessage PostPerson_Activity(int id, int projectId, Permissions model)
        {
            throw new NotImplementedException();
        }

        [DELETE("activities/{activityId}/discussions/{id}/people")]
        public HttpResponseMessage DeletePerson_Activity(int id, int projectId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion Activities
    }
}