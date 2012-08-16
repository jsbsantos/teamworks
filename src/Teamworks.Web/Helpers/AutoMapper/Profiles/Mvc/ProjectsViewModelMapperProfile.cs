using System.Collections.Generic;
using AutoMapper;
using Teamworks.Core;
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

            Mapper.CreateMap<Project, ProjectsViewModel.Project>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s=>s.People, o => o.NullSubstitute(new List<PersonViewModel>()))
                .ForMember(s => s.People, o => o.Ignore());
        }
    }
}