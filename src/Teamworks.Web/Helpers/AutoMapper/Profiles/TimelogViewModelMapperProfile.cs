using AutoMapper;
using Teamworks.Core;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles
{
    public class TimelogViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Timelog, TimelogViewModel>()
                .ForMember(r => r.Person, o => o.Ignore());
            
            Mapper.CreateMap<Timelog, ViewModels.Api.TimelogViewModel>()
                 .ForMember(s => s.Activity, o => o.Ignore());

        }
    }
}