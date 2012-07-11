using Raven.Json.Linq;
using Teamworks.Core;
using Teamworks.Core.Oauth2;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.Teamworks
{
    public static class PersonExtensions
    {
        public static void SetOpenId(this Person person, string provider, string claim)
        {
            var metadata = Global.Database.CurrentSession.Advanced.GetMetadataFor(person);
            metadata[OpenId.ProviderKey] = provider;
            metadata[OpenId.ClaimKey] = claim;
        }

        public static string GetOpenIdProvider(this Person person)
        {
            var metadata = Global.Database.CurrentSession.Advanced.GetMetadataFor(person);
            return metadata[OpenId.ProviderKey].Value<string>();
        }

        public static string GetOpenIdClaim(this Person person)
        {
            var metadata = Global.Database.CurrentSession.Advanced.GetMetadataFor(person);
            return metadata[OpenId.ClaimKey].Value<string>();
        }
    }
}