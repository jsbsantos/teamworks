﻿using System.Collections.Generic;
using AutoMapper;
using Teamworks.Core;
using Teamworks.Core.Projects;
using Teamworks.Web.Models;

namespace Teamworks.Web.Helpers
{
    public class AutoMapperSetup
    {
        /*
         AutoMapper uses "Convention over configuration" which means properties with the same name 
         will be auto-mapped to each other.         
         */

        public static void RegisterMappers()
        {
            Mapper.CreateMap<ProjectModel, Project>();

            Mapper.CreateMap<Project, ProjectModel>()
                .ForMember(src => src.Tasks,
                           opt => opt.MapFrom(src =>
                                              Mapper.Map<IList<Task>, IList<TaskModel>>(
                                                  Global.Raven.CurrentSession.Load<Task>(src.Tasks))))
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));

            Mapper.CreateMap<TaskModel, Task>();

            Mapper.CreateMap<Task, TaskModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Identifier));
        }
    }
}