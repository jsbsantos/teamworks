using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.AutoMapper.ValueResolvers;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles
{
    public class PersonViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            // Api
            Mapper.CreateMap<Person, ViewModels.Api.PersonViewModel>()
               .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()));
            
            //MVC
            Mapper.CreateMap<Person, ActivityViewModelComplete.AssignedPersonViewModel>()
               .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
               .ForMember(s => s.Assigned, o => o.Ignore())
               .ForMember(s => s.Gravatar, o => o.ResolveUsing<GravatarResolver>()
                                                     .FromMember("Email")); ;

            Mapper.CreateMap<Person, EntityViewModel>();
        }
    }
}