using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.ViewModels.Api;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Api
{
    public class TimelogViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Timelog, TimelogViewModel>()
                .ForMember(s => s.Activity, o => o.Ignore());

        }
    }
}