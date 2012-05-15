using System.Dynamic;
using System.Net;
using System.Net.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Teamworks.Core.People;
using Teamworks.Web.Controllers.Base;
using Teamworks.Web.Models;
using AuthenticationManager = Teamworks.Core.Authentication.AuthenticationManager;

namespace Teamworks.Web.Controllers.Api
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/login")]
    [System.Web.Http.AllowAnonymous]
    public class AccountController : RavenApiController
    {
        //GET: /Account/

        public HttpResponseMessage Post(string username, string password)
        {
            dynamic dyn = new ExpandoObject();
            dyn.Username = username;
            dyn.Password = password;

            var handler = AuthenticationManager.Get("Basic");
            NetworkCredential cred = AuthenticationManager.GetCredentials("BasicWeb", dyn);
            //todo Add NLog message
            if (handler != null && handler.IsValid(cred))
            {
                var token = Token.Forge(cred.UserName);
                DbSession.Store(token);
                //todo return http code 200 vs 201
                return new HttpResponseMessage<TokenModel>(new TokenModel() { Token = token.Value }, HttpStatusCode.OK);
            }
            
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);;
        }

    }
}
