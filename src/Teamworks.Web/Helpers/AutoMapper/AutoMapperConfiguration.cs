using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc;
using Teamworks.Web.ViewModels.Api;
using Message = Teamworks.Web.ViewModels.Api.Message;

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
            Mapper.AddProfile(new EntityViewModelMapperProfile());
            Mapper.AddProfile(new ProjectViewModelMapperProfile());
            Mapper.AddProfile(new ProjectsViewModelMapperProfile());
            Mapper.AddProfile(new ActivityViewModelMapperProfile());
            Mapper.AddProfile(new PersonViewModelMapperProfile());
            Mapper.AddProfile(new RegisterTimelogsViewModelMapperProfile());
            Mapper.AddProfile(new TimelogViewModelMapperProfile());


            // todo change all this for separated profiles

            Mapper.CreateMap<ProjectViewModel, Project>();
            Mapper.CreateMap<Project, ProjectViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));

            Mapper.CreateMap<ActivityViewModel, Activity>();
            Mapper.CreateMap<Activity, ActivityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));

            Mapper.CreateMap<TimelogViewModel, Timelog>();
            Mapper.CreateMap<Timelog, TimelogViewModel>();

            Mapper.CreateMap<DiscussionViewModel, Discussion>();
            Mapper.CreateMap<Discussion, DiscussionViewModel>()
                .ForMember(src => src.Content, opt => opt.MapFrom(src => src.Content ?? ""))
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));

            Mapper.CreateMap<Message, Core.Message>();
            Mapper.CreateMap<Core.Message, Message>();

            Mapper.CreateMap<PersonViewModel, Person>();
            Mapper.CreateMap<Person, PersonViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));
        }
    }
}