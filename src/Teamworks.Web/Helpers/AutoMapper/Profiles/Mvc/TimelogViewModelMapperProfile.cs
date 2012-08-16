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
            Mapper.CreateMap<Timelog, TimelogViewModel>()
                .ForMember(r => r.Person, o => o.Ignore());
        }
    }
}