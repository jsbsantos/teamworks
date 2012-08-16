using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Api
{
    public class PersonViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Person, PersonViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));
        }
    }
}