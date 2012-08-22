﻿using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Teamworks.Core;
using Teamworks.Web.Helpers;
using Teamworks.Web.Helpers.AutoMapper;
using Teamworks.Web.Helpers.Extensions.Mvc;
using Teamworks.Web.ViewModels.Mvc;

namespace Teamworks.Web.Controllers.Mvc
{
    [RoutePrefix("profiles")]
    public class ProfilesController : AppController
    {
        [GET("{id?}")]
        public ActionResult Get(int? id)
        {
            var personViewModel = new ProfileViewModel();
            var person = id.HasValue
                             ? DbSession.Load<Person>(id)
                             : DbSession.GetCurrentPerson();

            if (person == null)
                return HttpNotFound();
                
            if (person.Id == DbSession.GetCurrentPersonId())
                personViewModel.IsMyProfile = true;

            personViewModel.Person = person.MapTo<PersonViewModel>();
            return View("View", personViewModel);
        }

        [POST("")]
        public ActionResult Edit(ProfileViewModel.Input model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToRoute("profiles_get");
            }

            var person = DbSession.GetCurrentPerson();
            model.MapPropertiesToInstance(person);

            if (Request.IsAjaxRequest())
            {
                return new JsonNetResult {
                               Data = person.MapTo<PersonViewModel>()
                           };
            }
            return RedirectToRoute("profiles_get");
        }
    }
}