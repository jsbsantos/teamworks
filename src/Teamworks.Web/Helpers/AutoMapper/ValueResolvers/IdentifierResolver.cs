using AutoMapper;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.AutoMapper.ValueResolvers
{
    public class IdentifierResolver : ValueResolver<string, int>
    {
        protected override int ResolveCore(string source)
        {
            return source.ToIdentifier();
        }
    }
}