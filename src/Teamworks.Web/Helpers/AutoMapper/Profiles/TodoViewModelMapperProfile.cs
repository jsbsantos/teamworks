using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper.Profiles
{
    public class TodoViewModelMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Todo, TodoViewModel>();

            Mapper.CreateMap<Todo, TodoViewModel.Output>()
                .ForMember(r => r.Person, o => o.MapFrom(s => new EntityViewModel() {Id = s.Person.ToIdentifier()}));

            Mapper.CreateMap<TodoViewModel, Todo>();
        }
    }
}