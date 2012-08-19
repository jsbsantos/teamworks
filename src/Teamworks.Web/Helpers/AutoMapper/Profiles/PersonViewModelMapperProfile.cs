using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles
{
    public class PersonViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            // Api
            Mapper.CreateMap<Person, ViewModels.Api.PersonViewModel>()
               .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));
        }
    }
}