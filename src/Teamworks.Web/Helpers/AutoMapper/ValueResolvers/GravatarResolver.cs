using AutoMapper;

namespace Teamworks.Web.Helpers.AutoMapper.ValueResolvers
{
    public class GravatarResolver : ValueResolver<string, string>
    {
        protected override string ResolveCore(string source)
        {
            const string url = "http://www.gravatar.com/avatar/";
            return url + Utils.Hash(source) +"?r=g";
        }
    }
}