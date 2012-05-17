using System;
using Newtonsoft.Json;

namespace Teamworks.Core.People
{
    public class Token : Entity
    {
        private static int _timeout;

        static Token()
        {
            _timeout = 20;
        }

        public string Id { get; private set; }

        [JsonIgnore]
        public string Value
        {
            get { return Id.Replace("token/", ""); }
        }

        [JsonIgnore]
        public static int Timeout
        {
            get { return _timeout; }
            set
            {
                if (_timeout < 1)
                {
                    throw new ArgumentException("Timeout");
                }
                _timeout = value;
            }
        }

        public string Person { get; set; }

        //todo set expiration date
        /*
            //to be used with Raven Expiration Bundle
            //makes the used session active for the next <timeout> minutes
            Global.Raven.CurrentSession.Advanced.GetMetadataFor(token)["Raven-Expiration-Date"] =
                new RavenJValue(DateTime.UtcNow.AddMinutes(Timeout));         
         */

        public static Token Forge(string person)
        {
            var token = new Token();
            token.Id = "token/" + Guid.NewGuid().ToString("N");
            //todo remove prepended text "people/"
            token.Person = "people/" + person;
            return token;
        }
    }
}