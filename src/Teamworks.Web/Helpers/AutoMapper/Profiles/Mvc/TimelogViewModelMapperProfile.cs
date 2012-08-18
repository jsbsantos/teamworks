using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc
{
    public class TimelogViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<Timelog_Filter.Result, TimelogViewModel>()
                .ForMember(r => r.Profile, o=> o.Ignore());
            /*
            Mapper.CreateMap<Timelog, ActivityViewModel.TimelogViewModel>()
                .ForMember(r => r.Person, o => o.Ignore());
             */
        }
    }
}