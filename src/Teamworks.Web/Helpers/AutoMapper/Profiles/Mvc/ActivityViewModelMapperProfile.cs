using AutoMapper;
using Teamworks.Core;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc
{
    public class ActivityViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Activity, ActivityViewModelComplete>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier))
                .ForMember(s => s.Dependencies, o => o.Ignore())
                .ForMember(s => s.Timelogs, o => o.Ignore());

            Mapper.CreateMap<Activity, ActivityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier));
            Mapper.CreateMap<ActivityViewModel, Activity>();

            Mapper.CreateMap<Activity, DependencyActivityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier))
                .ForMember(s => s.Dependency, o => o.MapFrom(r => false));
            ;
        }
    }
}