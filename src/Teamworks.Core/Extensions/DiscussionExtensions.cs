using System.Collections.Generic;
using Raven.Client;

namespace Teamworks.Core.Extensions
{
    public static class DiscussionExtensions
    {
        private class EntityWithDiscussion : Entity
        {
            public IList<string> Discussions { get; set; }
        }

        public static void Delete(this Discussion discussion, IDocumentSession session)
        {
            var entity = session.Load<EntityWithDiscussion>(discussion.Entity);
            entity.Discussions.Remove(discussion.Id);
            session.Delete(discussion);
        }
    }
}