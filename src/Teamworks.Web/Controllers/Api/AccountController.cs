using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Teamworks.Core.People;
using Teamworks.Core.Services;
using Teamworks.Web.Models;

namespace Teamworks.Web.Controllers.Api
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    [DefaultHttpRouteConvention]
    public class AccountController : RavenApiController
    {
        public HttpResponseMessage Post(string username, string password)
        {
            dynamic dyn = new ExpandoObject();
            dyn.Username = username;
            dyn.Password = password;

            Person person;
            if (Global.Authentication["Basic"].IsValid(dyn, out person))
            {
                Token token = Token.Forge(person.Id);
                DbSession.Store(token);
                //todo return http code 200 vs 201
                return new HttpResponseMessage<TokenModel>(new TokenModel {Token = token.Value}, HttpStatusCode.OK);
            }

            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
    }
}