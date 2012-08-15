using Raven.Client;

namespace Teamworks.Core.Business
{
    public class BusinessService
    {
        public IDocumentSession DbSession { protected get; set; }
    }
}