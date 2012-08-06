using AutoMapper;
using Teamworks.Core;
using Teamworks.Web.Helpers.AutoMapper.ValueResolvers;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc
{
    public class PersonViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Person, PersonViewModel>()
             .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier));

            Mapper.CreateMap<Person, PersonViewModel>()
             .ForMember(s => s.Id, o => o.MapFrom(d => d.Identifier))
             .ForMember(s => s.Gravatar, o => o.ResolveUsing<GravatarResolver>()
                 .FromMember("Email"));
        }

    }
}