using System.Collections.Generic;
using AutoMapper;
using Teamworks.Core.Services;
using Teamworks.Web.Helpers.Teamworks;
using Teamworks.Web.Models.Api;
using Teamworks.Web.Models.Api.DryModels;

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
            #region Project Mappings

            Mapper.CreateMap<Project, Core.Project>();
            Mapper.CreateMap<Core.Project, Project>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Activities,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<IList<Core.Activity>, IList<Activity>>(
                                   Global.Database.CurrentSession.Load<Core.Activity>(src.Activities))))
                .ForMember(src => src.Discussions,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<IList<Core.Discussion>, IList<Discussion>>(
                                   Global.Database.CurrentSession.Load<Core.Discussion>(src.Discussions))));

            Mapper.CreateMap<Core.Project, DryProject>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            #endregion

            #region Activities Mappings

            Mapper.CreateMap<Activity, Core.Activity>();
            Mapper.CreateMap<Core.Activity, Activity>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Project, opt => opt.MapFrom(src => src.Project.Identifier()))
                .ForMember(src => src.Timelogs, opt => opt.MapFrom(src => src.Timelogs));
            Mapper.CreateMap<Core.Activity, DryActivity>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            #endregion

            #region Timelog Mappings

            Mapper.CreateMap<Timelog, Core.Timelog>();
            Mapper.CreateMap<Core.Timelog, Timelog>()
                .ForMember(src => src.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-ddTHH:mm:ssZ")))
                .ForMember(src => src.Person, opt => opt.MapFrom(src => Global.Database.CurrentSession.Load<Core.Person>(src.Person)));

            #endregion

            #region Discussion Mappings

            Mapper.CreateMap<DryDiscussion, Core.Discussion>();
            Mapper.CreateMap<Core.Discussion, DryDiscussion>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Person,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<Core.Person, DryPerson>(Global.Database.CurrentSession.Load<Core.Person>(src.Person))));

            Mapper.CreateMap<Discussion, Core.Discussion>();
            Mapper.CreateMap<Core.Discussion, Discussion>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier))
                .ForMember(src => src.Person,
                           opt =>
                           opt.MapFrom(
                               src =>
                               Mapper.Map<Core.Person, DryPerson>(Global.Database.CurrentSession.Load<Core.Person>(src.Person))));

            #endregion

            #region Message Mappings

            Mapper.CreateMap<Message, Core.Message>()
                .ForMember(src => src.Content, opt => opt.MapFrom(src => src.Content));

            Mapper.CreateMap<Core.Message, Message>()
                .ForMember(src => src.Content, opt => opt.MapFrom(src => src.Content))
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