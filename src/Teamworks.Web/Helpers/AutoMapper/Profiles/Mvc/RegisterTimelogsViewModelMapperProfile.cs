using AutoMapper;
using Teamworks.Core;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc
{
    public class RegisterTimelogsViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Timelog, RegisterTimelogsViewModel.Timelog>()
                .ForMember(s => s.Activity, o => o.Ignore());
        }
    }
}