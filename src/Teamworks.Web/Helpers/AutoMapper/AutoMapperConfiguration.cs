using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Teamworks.Web.Helpers.AutoMapper.Profiles.Mvc;

namespace Teamworks.Web.Helpers.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        /*
        AutoMapper uses "Convention over configuration" which means properties with the same name 
        will be auto-mapped to each other.         
        */

        public static void Configure()
        {
            var targetAssembly = Assembly.GetExecutingAssembly(); // or whichever
            var subtypes = targetAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Profile)));

            foreach (var subtype in subtypes)
            {
                Mapper.AddProfile(Activator.CreateInstance(subtype) as Profile);
            }
        }
    }
}