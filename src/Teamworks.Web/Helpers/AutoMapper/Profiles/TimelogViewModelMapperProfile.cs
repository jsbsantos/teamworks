using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services.RavenDb.Indexes;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles
{
    public class TimelogViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            //todo remove
            //Mapper.CreateMap<Timelog_Filter.Result, TimelogViewModel>()
            //    .ForMember(r => r.Person, o=> o.Ignore());

            Mapper.CreateMap<Timelog, TimelogViewModel>()
                .ForMember(r => r.Person, o => o.Ignore());

        }
    }
}