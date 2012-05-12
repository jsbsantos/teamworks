using System.Linq;
using System.Net;
using Teamworks.Core.People;

namespace Teamworks.Core.Authentication {
    public abstract class WebAuthentication : IAuthenticationHandler {
        public bool IsValid(NetworkCredential credential) {
            var person =
                Global.Raven.CurrentSession.Query<Person>().Where(
                    p => p.Username == credential.UserName)
                    .ToArray().FirstOrDefault();
            return person != null && person.IsThePassword(credential.Password);
        }

        public abstract NetworkCredential GetCredentials(dynamic token);
    }
}