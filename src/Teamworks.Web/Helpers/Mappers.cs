using System.Collections.Generic;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models;
using Teamworks.Web.Models.DryModels;
using Message = Teamworks.Web.Models.Message;
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
            #region Parent Mappings

            Mapper.CreateMap<Project, Core.Project>();
            Mapper.CreateMap<Core.Project, Project>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Tasks,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<IList<Core.Task>, IList<Task>>(
                                   Global.Database.CurrentSession.Load<Core.Task>(src.Tasks))))
                .ForMember(src => src.Discussions,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<IList<Core.Discussion>, IList<Discussions>>(
                                   Global.Database.CurrentSession.Load<Core.Discussion>(src.Discussions))));

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

            #region Discussions Mappings

            Mapper.CreateMap<DryDiscussions, Core.Discussion>();
            Mapper.CreateMap<Core.Discussion, DryDiscussions>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Person,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<Core.Person, DryPerson>(Global.Database.CurrentSession.Load<Core.Person>(src.Person))));

            Mapper.CreateMap<Discussions, Core.Discussion>();
            Mapper.CreateMap<Core.Discussion, Discussions>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Person,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<Core.Person, DryPerson>(Global.Database.CurrentSession.Load<Core.Person>(src.Person))));

            #endregion

            #region Message Mappings

            Mapper.CreateMap<Message, Core.Message>()
                .ForMember(src => src.Content, opt => opt.MapFrom(src => src.Text));

            Mapper.CreateMap<Core.Message, Message>()
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