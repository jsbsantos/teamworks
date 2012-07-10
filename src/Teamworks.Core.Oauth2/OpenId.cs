using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace Teamworks.Core.Oauth2
{
    public class OpenIdResult
    {
        public int State { get; set; }
        public string Email { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string ClaimedIdentifier { get; set; }
    }

    public class OpenId
    {
        public static string ClaimKey
        {
            get { return "authOpenIdClaim"; }
        }

        public static string ProviderKey
        {
            get { return "authOpenIdProvider"; }
        }

        public OpenIdResult Authenticate(string provider)
        {
            //The Request
            using (OpenIdRelyingParty openid = new OpenIdRelyingParty())
            {
                var response = openid.GetResponse();
                var obj = new OpenIdResult();
                if (response == null)
                {
                    IAuthenticationRequest request = openid.CreateRequest(provider);

                    var fetch = new FetchRequest();
                    fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                    fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
                    fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);
                    request.AddExtension(fetch);

                    request.RedirectToProvider();
                    obj.State = 0;
                    return obj;
                }
                if (response.Status == AuthenticationStatus.Authenticated)
                {
                    var responseresult = response.GetExtension<FetchResponse>();
                    if (responseresult != null)
                    {
                        obj.State = 1;
                        obj.Email = responseresult.GetAttributeValue(WellKnownAttributes.Contact.Email);
                        obj.First = responseresult.GetAttributeValue(WellKnownAttributes.Name.First);
                        obj.Last = responseresult.GetAttributeValue(WellKnownAttributes.Name.Last);
                        obj.ClaimedIdentifier = response.ClaimedIdentifier;
                        return obj;
                    }
                }
                obj.State = -1;
                return obj;
            }
        }
    }
}