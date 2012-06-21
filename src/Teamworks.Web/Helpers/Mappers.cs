using System.Collections.Generic;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Teamworks.Web.Models.DryModels;
using Board = Teamworks.Web.Models.Board;
using Person = Teamworks.Web.Models.Person;
using Project = Teamworks.Web.Models.Project;
using Task = Teamworks.Web.Models.Task;
using Timelog = Teamworks.Web.Models.Timelog;

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
            #region Entity Mappings

            Mapper.CreateMap<Project, Core.Project>();
            Mapper.CreateMap<Core.Project, Project>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Tasks,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<IList<Core.Task>, IList<Task>>(
                                   Global.Database.CurrentSession.Load<Core.Task>(src.Tasks))))
                .ForMember(src => src.Threads,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<IList<Core.Board>, IList<Board>>(
                                   Global.Database.CurrentSession.Load<Core.Board>(src.Boards))));

            Mapper.CreateMap<Core.Project, DryProject>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            #endregion

            #region Task Mappings

            Mapper.CreateMap<Task, Core.Task>();
            Mapper.CreateMap<Core.Task, Task>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Project, opt => opt.MapFrom(src => src.Project.Identifier()))
                .ForMember(src => src.Timelog, opt => opt.MapFrom(src => src.Timelogs));
            Mapper.CreateMap<Core.Task, DryTask>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            #endregion

            #region Timelogs Mappings

            Mapper.CreateMap<Timelog, Core.Timelog>();
            Mapper.CreateMap<Core.Timelog, Timelog>()
                .ForMember(src => src.Person, opt => opt.MapFrom(src => Global.Database.CurrentSession.Load<Core.Person>(src.Person)));

            #endregion

            #region Board Mappings

            Mapper.CreateMap<DryBoard, Core.Board>();
            Mapper.CreateMap<Core.Board, DryBoard>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Person,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<Core.Person, DryPerson>(Global.Database.CurrentSession.Load<Core.Person>(src.Person))));

            Mapper.CreateMap<Board, Core.Board>();
            Mapper.CreateMap<Core.Board, Board>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Person,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<Core.Person, DryPerson>(Global.Database.CurrentSession.Load<Core.Person>(src.Person))));

            #endregion

            #region Reply Mappings

            Mapper.CreateMap<Reply, Core.Message>()
                .ForMember(src => src.Content, opt => opt.MapFrom(src => src.Text));

            Mapper.CreateMap<Core.Message, Reply>()
                .ForMember(src => src.Text, opt => opt.MapFrom(src => src.Content))
                .ForMember(src => src.Person,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<Core.Person, DryPerson>(Global.Database.CurrentSession.Load<Core.Person>(src.Person))));

            #endregion

            #region Person Mappings

            Mapper.CreateMap<DryPerson, Core.Person>();
            Mapper.CreateMap<Core.Person, DryPerson>();
            Mapper.CreateMap<Person, Core.Person>();
            Mapper.CreateMap<Core.Person, Person>();

            #endregion
        }
    }
}