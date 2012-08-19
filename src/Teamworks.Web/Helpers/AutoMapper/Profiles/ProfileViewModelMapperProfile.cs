using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.AutoMapper.ValueResolvers;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles
{
    public class ProfileViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Person, PersonViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s => s.Gravatar, o => o.ResolveUsing<GravatarResolver>()
                                                     .FromMember("Email"));

            Mapper.CreateMap<ProfileViewModel.Input, Person>();
        }
    }
}