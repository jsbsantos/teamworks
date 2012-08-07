using AutoMapper;
using Teamworks.Core;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc
{
    public class EntityViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Person, EntityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier));
            Mapper.CreateMap<Project, EntityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier));
            Mapper.CreateMap<Activity, EntityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier));
            Mapper.CreateMap<Discussion, EntityViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier));
        }
    }
}