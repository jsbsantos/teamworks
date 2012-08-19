using System.Collections.Generic;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles
{
    public class DiscussionViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            // Mvc
            Mapper.CreateMap<Discussion, DiscussionViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s => s.Messages, o => o.UseValue(new List<DiscussionViewModel.Message>()));

            Mapper.CreateMap<Discussion.Message, DiscussionViewModel.Message>()
                .ForMember(s => s.Person, o => o.Ignore());

            // Api
            Mapper.CreateMap<Discussion, ViewModels.Api.DiscussionViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s => s.Messages, o => o.MapFrom(d => d.Messages.MapTo<ViewModels.Api.DiscussionViewModel>()));

            Mapper.CreateMap<Discussion, ViewModels.Api.DiscussionViewModel.Message>()
                .ForMember(s => s.Person, o => o.MapFrom(d => d.Person.ToIdentifier()));

        }
    }
}