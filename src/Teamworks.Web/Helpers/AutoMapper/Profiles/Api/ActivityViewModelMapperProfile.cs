using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Api
{
    public class ActivityViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Activity, ActivityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s => s.Project, o => o.MapFrom(d => d.Project.ToIdentifier()));
        }
    }
}