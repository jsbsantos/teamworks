using System.Collections.Generic;
using AutoMapper;
using Teamworks.Core.People;
using Teamworks.Core.Projects;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Teamworks.Web.Models.DryModels;

namespace Teamworks.Web.Helpers
{
    public static class Mappers
    {
        /*
        AutoMapper uses "Convention over configuration" which means properties with the same name 
        will be auto-mapped to each other.         
        */

        public static void RegisterMappers()
        {
            #region Entity Mappings
            Mapper.CreateMap<ProjectModel, Project>();
            Mapper.CreateMap<Project, ProjectModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Tasks,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<IList<Task>, IList<TaskModel>>(
                                   Global.Raven.CurrentSession.Load<Task>(src.Tasks))))
                .ForMember(src => src.Discussions,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<IList<Topic>, IList<TopicModel>>(
                                   Global.Raven.CurrentSession.Load<Topic>(src.Discussions))));

            Mapper.CreateMap<Project, DryProjectModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));
            #endregion

            #region Task Mappings
            Mapper.CreateMap<TaskModel, Task>();
            Mapper.CreateMap<Task, TaskModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Project, opt => opt.MapFrom(src => src.Project.Identifier()))
                .ForMember(src => src.Timelog, opt => opt.MapFrom(src => src.Timelog));
            Mapper.CreateMap<Task, DryTaskModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));
            #endregion

            #region TimeEntry Mappings
            Mapper.CreateMap<TimeEntryModel, TimeEntry>();
            Mapper.CreateMap<TimeEntry, TimeEntryModel>();
            #endregion

            #region Topic Mappings
            Mapper.CreateMap<DryTopicModel, Topic>();
            Mapper.CreateMap<Topic, DryTopicModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Person, opt => opt.MapFrom(src => Mapper.Map<Person, DryPersonModel>(Global.Raven.CurrentSession.Load<Person>(src.Person))));

            Mapper.CreateMap<TopicModel, Topic>();
            Mapper.CreateMap<Topic, TopicModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Person, opt => opt.MapFrom(src => Mapper.Map<Person,DryPersonModel>(Global.Raven.CurrentSession.Load<Person>(src.Person))));
            #endregion

            #region Message Mappings
            Mapper.CreateMap<MessageModel, Message>();
            Mapper.CreateMap<Message, MessageModel>()
                .ForMember(src => src.Person, opt => opt.MapFrom(src => Mapper.Map<Person, DryPersonModel>(Global.Raven.CurrentSession.Load<Person>(src.Person))));
            #endregion

            #region Person Mappings
            Mapper.CreateMap<DryPersonModel, Person>();
            Mapper.CreateMap<Person, DryPersonModel>();  
            Mapper.CreateMap<PersonModel, Person>();
            Mapper.CreateMap<Person, PersonModel>();
            #endregion
        }
    }
}