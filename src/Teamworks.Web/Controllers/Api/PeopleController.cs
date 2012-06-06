using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Teamworks.Core.People;
using Teamworks.Core.Projects;
using Teamworks.Web.Helpers.Extensions;

namespace Teamworks.Web.Controllers.Api
{
    [RoutePrefix("api/projects/{projectid}")]
    public class PeopleController : RavenApiController
    {
        [GET("people")]
        public List<DocumentPermission> Get(int projectid)
        {
            var project = Get<Project>(projectid);
            return DbSession.GetAuthorizationFor(project).Permissions;
        }

        [POST("people")]
        public HttpResponseMessage<PersonModel> Post([ModelBinder(typeof (TypeConverterModelBinder))] int projectid,
                                                     string name)
        {
            var project = Get<Project>(projectid);

            // todo check if user exists
            var person = Get<Person>("people/" + name);

            return null;
        }
    }

    public class PersonModel
    {
    }
}