﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using AttributeRouting;
using AttributeRouting.Web.Http;
using AutoMapper;
using Teamworks.Web.Helpers.Api;
using Teamworks.Web.Models;
using Teamworks.Web.Models.Api;
using Teamworks.Web.Models.Api.DryModels;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api/projects/{projectid}")]
    public class DiscussionsController : RavenApiController
    {
        #region Project Discussion

        private Core.Project LoadProject(int projectid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Discussions)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return project;
        }

        [GET("discussions")]
        public IEnumerable<DryDiscussion> GetProjectDiscussion(int projectid)
        {
            return
                new List<DryDiscussion>(
                    DbSession.Load<Core.Discussion>(LoadProject(projectid).Discussions).Select(Mapper.Map<Core.Discussion, DryDiscussion>));
        }

        [GET("discussions/{id}")]
        public Discussion GetProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var topic = DbSession.Load<Core.Discussion>(id);
            if (topic == null || !topic.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return Mapper.Map<Core.Discussion, Discussion>(topic);
        }

        [POST("discussions")]
        public HttpResponseMessage PostProjectDiscussion(
             int projectid,
            Discussion model)
        {
            var project = LoadProject(projectid);

            var discussion = Core.Discussion.Forge(model.Name, model.Content, project.Id, Request.GetCurrentPersonId());

            DbSession.Store(discussion);
            project.Discussions.Add(discussion.Id);

            var response = Mapper.Map<Core.Discussion, Discussion>(discussion);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("discussions/{id}")]
        public HttpResponseMessage PutProjectDiscussion( int id,
                                                         int projectid,
                                                        Discussion model)
        {
            var project = LoadProject(projectid);
            var discussion = DbSession.Load<Core.Discussion>(id);
            if (discussion == null || !discussion.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            discussion.Content = model.Content;

            var response = Mapper.Map<Core.Discussion, Discussion>(discussion);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [DELETE("discussions/{id}")]
        public HttpResponseMessage DeleteProjectDiscussion(int id, int projectid)
        {
            var project = LoadProject(projectid);
            var discussion = DbSession.Load<Core.Discussion>(id);
            if (discussion == null || !discussion.Entity.Equals(project.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            project.Discussions.Remove(discussion.Id);
            DbSession.Delete(discussion);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion

        #region Activities Discussion

        private Core.Activity LoadTask(int projectid, int activityid)
        {
            var project = DbSession
                .Include<Core.Project>(p => p.Activities)
                .Load<Core.Project>(projectid);

            if (project == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var activity = DbSession
                .Include<Core.Activity>(t => t.Discussions)
                .Load<Core.Activity>(activityid);

            if (activity == null || !activity.Project.Equals(project.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return activity;
        }

        [GET("activities/{activityid}/discussions")]
        public IEnumerable<DryDiscussion> GetTaskDiscussion(int projectid, int activityid)
        {
            return
                new List<DryDiscussion>(
                    DbSession.Load<Core.Discussion>(LoadTask(projectid, activityid).Discussions).Select(
                        Mapper.Map<Core.Discussion, DryDiscussion>));
        }

        [GET("activities/{activityid}/discussions/{id}")]
        public Discussion GetTaskDiscussion(int id, int projectid, int activityid)
        {
            var task = LoadTask(projectid, activityid);
            var topic = DbSession.Load<Core.Discussion>(id);
            if (topic == null || !topic.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return Mapper.Map<Core.Discussion, Discussion>(topic);
        }

        [POST("activities/{activityid}/discussions/")]
        public HttpResponseMessage PostTaskDiscussion(
             int projectid,
             int activityid,
            Discussion model)
        {
            var task = LoadTask(projectid, activityid);

            Core.Discussion discussion = Core.Discussion.Forge(model.Name, model.Content, task.Id, Request.GetCurrentPersonId());
            DbSession.Store(discussion);
            task.Discussions.Add(discussion.Id);

            var response = Mapper.Map<Core.Discussion, Discussion>(discussion);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        /// <see cref="http://forums.asp.net/post/4855634.aspx" />
        [PUT("activities/{activityid}/discussions/{id}")]
        public HttpResponseMessage PutTaskDiscussion( int id,
                                                      int projectid,
                                                      int activityid,
                                                     Message model)
        {
            var task = LoadTask(projectid, activityid);
            var discussion = DbSession.Load<Core.Discussion>(id);
            if (discussion == null || !discussion.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            discussion.Content = model.Content;

            var response = Mapper.Map<Core.Discussion, Discussion>(discussion);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [DELETE("activities/{activityid}/discussions/{id}")]
        public HttpResponseMessage DeleteTaskDiscussion(int id, int projectid, int activityid)
        {
            var task = LoadTask(projectid, activityid);
            var discussion = DbSession.Load<Core.Discussion>(id);
            if (discussion == null || !discussion.Entity.Equals(task.Id))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            task.Discussions.Remove(discussion.Id);
            DbSession.Delete(discussion);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        #endregion Activities
    }
}