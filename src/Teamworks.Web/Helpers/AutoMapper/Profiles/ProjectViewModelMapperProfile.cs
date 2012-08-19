using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles
{
    public class ProjectViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Project, ProjectViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s => s.People, o => o.Ignore());

            Mapper.CreateMap<Activity, ProjectViewModel.Activity>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));

            Mapper.CreateMap<Discussion, ProjectViewModel.Discussion>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));

            // Api
            Mapper.CreateMap<Project, ViewModels.Api.ProjectViewModel>()
              .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));

        }
    }
}