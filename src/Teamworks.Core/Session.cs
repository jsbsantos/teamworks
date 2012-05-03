using System;
using Newtonsoft.Json;
using Raven.Abstractions.Data;
using Raven.Client.Linq;
using Raven.Json.Linq;
using Teamworks.Core.People;

namespace Teamworks.Core {
    public class Session : BaseEntity<Session> {
        static Session() {
            _timeout = 20;
        }

        [JsonIgnore]
        public static int Timeout {
            get { return _timeout; }
            set {
                if (_timeout < 1) {
                    throw new ArgumentException("Timeout");
                }
                _timeout = value;
            }
        }

        private static int _timeout;


        public Session() {
            //to be used with Raven Expiration Bundle
            //makes the used session active for the next <timeout> minutes
            Session.Advanced.GetMetadataFor(this)["Raven-Expiration-Date"] =
                new RavenJValue(DateTime.UtcNow.AddMinutes(Timeout));
        }

        public Person Person { get; set; }
        public string Id { get; set; }
    }
}