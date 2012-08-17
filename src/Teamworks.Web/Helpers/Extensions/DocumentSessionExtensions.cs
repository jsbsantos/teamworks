using System.Collections.Generic;
using System.Linq;
using Raven.Bundles.Authorization.Model;
using Raven.Client;
using Raven.Client.Authorization;
using Teamworks.Core;
using Teamworks.Core.Services;

namespace Teamworks.Web.Helpers.Extensions
{
    public static class DocumentSessionExtensions
    {
        public static Person GetPersonByEmail(this IDocumentSession session, string email)
        {
            return session.Query<Person>()
                .Where(p => p.Email == email).FirstOrDefault();
        }

        public static Person GetPersonByUsername(this IDocumentSession session, string username)
        {
            return session.Query<Person>()
                .Where(p => p.Username == username).FirstOrDefault();
        }

        public static Person GetPersonByLogin(this IDocumentSession session, string login)
        {
            return session.Query<Person>()
                .Where(p => p.Username == login || p.Email == login).FirstOrDefault();
        }
    }
}