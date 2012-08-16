using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc;

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
            // mvc
            // todo would make sense to add all of those automatically with an IoC
            /*Mapper.AddProfile(new EntityViewModelMapperProfile());
            Mapper.AddProfile(new ProjectViewModelMapperProfile());
            Mapper.AddProfile(new ProjectsViewModelMapperProfile());
            Mapper.AddProfile(new ActivityViewModelMapperProfile());
            Mapper.AddProfile(new PersonViewModelMapperProfile());
            Mapper.AddProfile(new ProfileViewModelMapperProfile());
            Mapper.AddProfile(new RegisterTimelogsViewModelMapperProfile());
            Mapper.AddProfile(new TimelogViewModelMapperProfile());
            */
            // api

            var targetAssembly = Assembly.GetExecutingAssembly(); // or whichever
            var subtypes = targetAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Profile)));

            foreach (var subtype in subtypes)
            {
                Mapper.AddProfile(Activator.CreateInstance(subtype) as Profile);
            }

            /*
            Mapper.AddProfile(new Profiles.Api.ProjectViewModelMapperProfile());
            Mapper.AddProfile(new Profiles.Api.ActivityViewModelMapperProfile());
            */
            /*
            Mapper.CreateMap<ProjectViewModel, VetoProject>();
            Mapper.CreateMap<VetoProject, ProjectViewModel>()
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
             */
        }
    }
}