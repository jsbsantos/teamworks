﻿using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Teamworks.Core;
using Teamworks.Core.Authentication;

namespace Teamworks.Web.Helpers.Api
{
    public static class HttpRequestMessageExtensions
    {
        public static Person GetCurrentPerson(this HttpRequestMessage request)
        {
            var principal = Thread.CurrentPrincipal;
            if (principal == null)
            {
                return null;
            }

            var person = principal.Identity as PersonIdentity;
            return person == null ? null : person.Person;
        }

        public static string GetCurrentPersonId(this HttpRequestMessage request)
        {
            var person = GetCurrentPerson(request);
            return person == null ? "" : person.Id;
        }

        public static void ThrowNotFound(this HttpRequestMessage request)
        {
            throw new HttpResponseException(request.CreateResponse(HttpStatusCode.NotFound));
        }

        public static void ThrowNotFoundIfNull(this HttpRequestMessage request, object obj)
        {
            if (obj == null)
            {
                throw new HttpResponseException(request.CreateResponse(HttpStatusCode.NotFound));
            }
        }
    }
}