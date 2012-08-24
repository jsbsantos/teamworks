using System.Linq;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles
{
    public class ActivityViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Activity, ActivityViewModelComplete>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s => s.Dependencies, o => o.Ignore())
                .ForMember(s => s.Timelogs, o => o.Ignore())
                .ForMember(s => s.People, o => o.Ignore())
                .ForMember(s => s.Discussions, o => o.Ignore());

            Mapper.CreateMap<Activity, ActivityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));

            Mapper.CreateMap<Activity, EntityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));

            Mapper.CreateMap<ActivityViewModel, Activity>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToId("activity")))
                .ForMember(s => s.Project, o => o.MapFrom(d => d.ProjectReference.Id.ToId("project")));

            Mapper.CreateMap<ActivityViewModel.Input, Activity>()
                .ForMember(s => s.Dependencies, o => o.MapFrom(d => d.Dependencies.Select(a => a.ToId("activity"))))
                .ForMember(s => s.Project, o => o.MapFrom(d => d.Project.ToId("project")))
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToId("activity")));

            Mapper.CreateMap<Activity, DependencyActivityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s => s.Dependency, o => o.MapFrom(r => false));

            // Api
            Mapper.CreateMap<Activity, ViewModels.Api.ActivityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s => s.Project, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s => s.People, o => o.MapFrom(d => d.People.Select(p => p.ToIdentifier())));


        }
    }
}