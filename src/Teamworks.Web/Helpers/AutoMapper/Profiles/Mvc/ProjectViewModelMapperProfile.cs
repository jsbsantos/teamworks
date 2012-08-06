using AutoMapper;
using Teamworks.Core;
using Teamworks.Web.Controllers.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc
{
    public class ProjectViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Project, ProjectViewModel.ProjectSummary>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier));

            Mapper.CreateMap<Activity, ProjectViewModel.Activity>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier));

            Mapper.CreateMap<Discussion, ProjectViewModel.Discussion>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier));
        }
    }
}