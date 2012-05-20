using System.Collections.Generic;
using AutoMapper;
using Teamworks.Core.Projects;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Extensions;
using Teamworks.Web.Models;

namespace Teamworks.Web.Helpers
{
    public static class Mappers
    {
        /*
        AutoMapper uses "Convention over configuration" which means properties with the same name 
        will be auto-mapped to each other.         
        */

        public static void RegisterMappers()
        {
            Mapper.CreateMap<ProjectModel, Project>();
            Mapper.CreateMap<Project, ProjectModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Tasks,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<IList<Task>, IList<TaskModel>>(
                                   Global.Raven.CurrentSession.Load<Task>(src.Tasks))));
            Mapper.CreateMap<Project, DryProjectModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            Mapper.CreateMap<TaskModel, Task>();
            Mapper.CreateMap<Task, TaskModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Project, opt => opt.MapFrom(src => src.Project.Identifier()))
                .ForMember(src => src.Timelog, opt => opt.MapFrom(src => src.Timelog));

            Mapper.CreateMap<TimeEntryModel, Timelog>();
            Mapper.CreateMap<Timelog, TimeEntryModel>();
        }
    }
}