using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Teamworks.Core.Authentication;
using Teamworks.Core.People;
using Teamworks.Web.Models;
using AuthenticationManager = Teamworks.Core.Authentication.AuthenticationManager;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/login")]
    [AllowAnonymous]
    public class AccountController : RavenApiController
    {
        public HttpResponseMessage Post(string username, string password)
        {
            dynamic dyn = new ExpandoObject();
            dyn.Username = username;
            dyn.Password = password;

            IAuthenticationHandler handler = AuthenticationManager.Get("Basic");
            var credentials = AuthenticationManager.GetCredentials("BasicWeb", dyn);
            //todo Add NLog message
            if (handler != null && handler.IsValid(credentials))
            {
                var token = Token.Forge(credentials.UserName);
                DbSession.Store(token);
                //todo return http code 200 vs 201
                return new HttpResponseMessage<TokenModel>(new TokenModel {Token = token.Value}, HttpStatusCode.OK);
            }

            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            ;
        }
    }
}