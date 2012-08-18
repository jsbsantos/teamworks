using System.Collections.Generic;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc
{
    public class DiscussionViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Discussion, DiscussionViewModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id.ToIdentifier()))
                .ForMember(s => s.Messages, o => o.UseValue(new List<DiscussionViewModel.Message>()));

            Mapper.CreateMap<Discussion.Message, DiscussionViewModel.Message>()
                .ForMember(s => s.Person, o => o.Ignore());
        }
    }
}