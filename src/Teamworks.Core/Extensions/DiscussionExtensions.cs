using Raven.Client;

namespace Teamworks.Core.Extensions
{
    public static class DiscussionExtensions
    {
        public static void Delete(this Discussion discussion, IDocumentSession session)
        {
            session.Delete(discussion);
        }
    }
}