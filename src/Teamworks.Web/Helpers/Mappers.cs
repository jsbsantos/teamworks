﻿using AutoMapper;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models.Api;
using Teamworks.Web.Models.Api.DryModels;

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
            #region Project Mappings

            Mapper.CreateMap<Project, Core.Project>();
            Mapper.CreateMap<Core.Project, Project>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Token,
                           opt =>
                           opt.MapFrom(
                               src => string.Format("tw+{0}@teamworks.mailgun.org", src.Token(Global.CurrentPerson.Id))));

            Mapper.CreateMap<Core.Project, DryProject>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            #endregion

            #region Activities Mappings

            Mapper.CreateMap<Activity, Core.Activity>();
            Mapper.CreateMap<Core.Activity, Activity>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Project, opt => opt.MapFrom(src => src.Project.Identifier()))
                .ForMember(src => src.Timelogs, opt => opt.MapFrom(src => src.Timelogs));
            Mapper.CreateMap<Core.Activity, DryActivity>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            #endregion

            #region Timelog Mappings

            Mapper.CreateMap<Timelog, Core.Timelog>();
            Mapper.CreateMap<Core.Timelog, Timelog>()
                .ForMember(src => src.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-ddTHH:mm:ssZ")));

            #endregion

            #region Discussion Mappings

            Mapper.CreateMap<DryDiscussion, Core.Discussion>();
            Mapper.CreateMap<Core.Discussion, DryDiscussion>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            Mapper.CreateMap<Discussion, Core.Discussion>();
            Mapper.CreateMap<Core.Discussion, Discussion>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));
            #endregion

            #region Message Mappings

            Mapper.CreateMap<Message, Core.Message>()
                .ForMember(src => src.Content, opt => opt.MapFrom(src => src.Content));

            Mapper.CreateMap<Core.Message, Message>()
                .ForMember(src => src.Content, opt => opt.MapFrom(src => src.Content));

            #endregion

            #region Person Mappings

            Mapper.CreateMap<DryPerson, Core.Person>();
            Mapper.CreateMap<Core.Person, DryPerson>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));
            Mapper.CreateMap<Person, Core.Person>();
            Mapper.CreateMap<Core.Person, Person>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            #endregion

            #region TodoList

            Mapper.CreateMap<TodoList, Core.TodoList>();
            Mapper.CreateMap<Core.TodoList, TodoList>();

            #endregion

            #region Todo

            Mapper.CreateMap<Todo, Core.Todo>();
            Mapper.CreateMap<Core.Todo, Todo>();

            #endregion
        }
    }
}