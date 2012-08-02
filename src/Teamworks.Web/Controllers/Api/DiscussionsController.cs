using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Attributes.Api;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models.Api;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Controllers.Api
{
    [SecureFor("/projects")]
    [RoutePrefix("api/projects/{projectid}")]
    public class DiscussionsController : RavenDbApiController
    {
        #region Project Discussion

        [GET("discussions")]
        public IEnumerable<DryDiscussion> Get(int projectid)
        {
            var project = DbSession.Load<Core.Project>(projectid);

            var discussions = DbSession.Query<Core.Discussion>()
                .Where(d => d.Entity == project.Id)
                .ToList();

            return Mapper.Map<IEnumerable<Core.Discussion>, IEnumerable<DryDiscussion>>(discussions);
        }

        [GET("discussions/{id}")]
        public Discussion Get(int id, int projectid)
        {
            var project = DbSession.Load<Core.Project>(projectid);

            var discussion = DbSession.Load<Core.Discussion>(id);
            if (discussion == null || discussion.Entity != project.Id)
            {
                Request.NotFound();
            }

            return Mapper.Map<Core.Discussion, Discussion>(discussion);
        }

        [POST("discussions")]
        public HttpResponseMessage Post(int projectid, Discussion model)
        {
            var project = DbSession.Load<Core.Project>(projectid);
            var discussion = Core.Discussion.Forge(model.Name, model.Content, project.Id, Request.GetCurrentPersonId());

            DbSession.Store(discussion);
            project.Discussions.Add(discussion.Id);

            var response = Mapper.Map<Core.Discussion, Discussion>(discussion);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [PUT("discussions/{id}")]
        public HttpResponseMessage Put(int id, int projectid, Discussion model)
        {
            var project = DbSession
                .Include<Core.Discussion>(p => p.Identifier == id)
                .Load<Core.Project>(projectid);

            var discussion = DbSession.Load<Core.Discussion>(id);
            if (discussion == null || discussion.Entity != project.Id)
            {
                Request.NotFound();
            }

            discussion.Content = model.Content;
            var response = Mapper.Map<Core.Discussion, Discussion>(discussion);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [DELETE("discussions/{id}")]
        public HttpResponseMessage Delete(int id, int projectid)
        {
            var project = DbSession
                .Include<Core.Discussion>(p => p.Identifier == id)
                .Load<Core.Project>(projectid);

            var discussion = DbSession.Load<Core.Discussion>(id);
            if (discussion == null || !discussion.Entity.Equals(project.Id))
            {
                Request.NotFound();
            }

            project.Discussions.Remove(discussion.Id);
            DbSession.Delete(discussion);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #region Person

        [GET("discussions/{id}/people")]
        public IEnumerable<Person> GetPeople_Project(int id, int projectid)
        {
            var project = DbSession.Load<Core.Project>(projectid);

            var discussion = DbSession.Load<Core.Discussion>(id);
            if (discussion == null || discussion.Entity != project.Id)
            {
                Request.NotFound();
            }

            return
                Mapper.Map<IEnumerable<Core.Person>, IEnumerable<Person>>(
                    DbSession.Load<Core.Person>(discussion.Subscribers));
        }

        [POST("discussions/{id}/people")]
        public HttpResponseMessage PostPerson_Project(int id, int projectid, Person model)
        {
            var project = DbSession.Load<Core.Project>(projectid);

            var discussion = DbSession.Load<Core.Discussion>(id);
            if (discussion == null || discussion.Entity != project.Id ||
                !DbSession.Query<Core.Person>().Any(p => p.Id.Equals(model.Id)))
            {
                Request.NotFound();
            }

            discussion.Subscribers.Add(model.Id);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [DELETE("discussions/{id}/people")]
        public HttpResponseMessage DeletePerson_Project(int id, int projectid, Person model)
        {
            var project = DbSession.Load<Core.Project>(projectid);

            var discussion = DbSession.Load<Core.Discussion>(id);
            if (discussion == null || discussion.Entity != project.Id ||
                !DbSession.Query<Core.Person>().Any(p => p.Id.Equals(model.Id)))
            {
                Request.NotFound();
            }

            discussion.Subscribers.Remove(model.Id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion

        #endregion

        //TODO
        #region Activities Discussion

        [GET("activities/{activityid}/discussions")]
        public IEnumerable<DryDiscussion> GetTaskDiscussions(int projectid, int activityid)
        {
            throw new NotImplementedException();
        }

        [GET("activities/{activityid}/discussions/{id}")]
        public Discussion GetTaskDiscussion(int id, int projectid, int activityid)
        {
            throw new NotImplementedException();
        }

        [POST("activities/{activityid}/discussions/")]
        public HttpResponseMessage PostTaskDiscussion(
            int projectid,
            int activityid,
            Discussion model)
        {
            throw new NotImplementedException();
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("activities/{activityid}/discussions/{id}")]
        public HttpResponseMessage PutTaskDiscussion(int id,
                                                     int projectid,
                                                     int activityid,
                                                     Message model)
        {
            throw new NotImplementedException();
        }

        [DELETE("activities/{activityid}/discussions/{id}")]
        public HttpResponseMessage DeleteTaskDiscussion(int id, int projectid, int activityid)
        {
            throw new NotImplementedException();
        }

        #region Person

        [GET("activities/{activityid}/discussions/{id}/people")]
        public IEnumerable<Person> GetPeople_Activity(int id, int projectid)
        {
            throw new NotImplementedException();
        }

        [POST("activities/{activityid}/discussions/{id}/people")]
        public HttpResponseMessage PostPerson_Activity(int id, int projectid, Permissions model)
        {
            throw new NotImplementedException();
        }

        [DELETE("activities/{activityid}/discussions/{id}/people")]
        public HttpResponseMessage DeletePerson_Activity(int id, int projectid)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion Activities
    }
}