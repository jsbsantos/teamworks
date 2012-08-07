using AutoMapper;
using Teamworks.Core.Services;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc
{
    public class ProjectsViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ProjectEntityCount.Result, ProjectsViewModel.Project>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Project.ToIdentifier()))
                .ForMember(s => s.People, o => o.Ignore());
        }
    }
}