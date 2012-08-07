using AutoMapper;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc;
using Teamworks.Web.Models.Api;

namespace Teamworks.Web.Helpers.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        /*
        AutoMapper uses "Convention over configuration" which means properties with the same name 
        will be auto-mapped to each other.         
        */

        public static void Configure()
        {
            // todo would make sense to add all of those automatically with an IoC
            Mapper.AddProfile(new ProjectViewModelMapperProfile());
            Mapper.AddProfile(new ProjectsViewModelMapperProfile());
            Mapper.AddProfile(new ActivityViewModelMapperProfile());
            Mapper.AddProfile(new PersonViewModelMapperProfile());



            // todo change all this for separated profiles

            #region Project Mappings

            Mapper.CreateMap<Project, Core.Project>();
            Mapper.CreateMap<Core.Project, Project>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            #endregion

            #region Activities Mappings

            Mapper.CreateMap<Activity, Core.Activity>();
            Mapper.CreateMap<Core.Activity, Activity>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Project, opt => opt.MapFrom(src => src.Project.ToIdentifier()));

            #endregion

            #region Timelog Mappings

            Mapper.CreateMap<Timelog, Core.Timelog>();
            Mapper.CreateMap<Core.Timelog, Timelog>()
                .ForMember(src => src.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-ddTHH:mm:ssZ")));

            #endregion

            #region Discussion Mappings

            Mapper.CreateMap<Discussion, Core.Discussion>();
            Mapper.CreateMap<Core.Discussion, Discussion>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Content, opt => opt.MapFrom(src => src.Content ?? ""));

            #endregion

            #region Message Mappings

            Mapper.CreateMap<Message, Core.Message>()
                .ForMember(src => src.Content, opt => opt.MapFrom(src => src.Content));

            Mapper.CreateMap<Core.Message, Message>()
                .ForMember(src => src.Content, opt => opt.MapFrom(src => src.Content));

            #endregion

            #region Person Mappings

            Mapper.CreateMap<Person, Core.Person>();
            Mapper.CreateMap<Core.Person, Person>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Gravatar, opt => opt.MapFrom(src => Person.GravatarUrl(src.Email)));

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