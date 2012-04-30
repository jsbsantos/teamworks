using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using System.Text;

namespace Teamworks.Core.Authentication
{
    public static class AuthenticationManager
    {
        private static string _defaultScheme;
        public static string DefaultAuthenticationScheme
        {
            get { return _defaultScheme; }
            set
            {
                if (_handlers.ContainsKey(value))
                    _defaultScheme = value;
                else
                    throw new ArgumentException("Scheme");
            }
        }

        private static Dictionary<string, IAuthenticationHandler> _handlers;

        static AuthenticationManager()
        {
            _handlers = new Dictionary<string, IAuthenticationHandler>();
        }
        public static bool Validate(string scheme, NetworkCredential credential)
        {
            if (!_handlers.ContainsKey(scheme))
                throw new ArgumentException("scheme");
            return _handlers[scheme].Validate(credential);
        }
        public static void Add(string scheme, IAuthenticationHandler handler)
        {
            _handlers.Add(scheme,handler);
        }
        public static void Remove(string scheme)
        {
            _handlers.Remove(scheme);
        }
        public static IEnumerable<string>  GetRegisteredSchemes()
        {
            return _handlers.Keys;
        }

        public static NetworkCredential GetCredentials(string scheme, dynamic token)
        {
            if (!_handlers.ContainsKey(scheme))
                throw new ArgumentException("scheme");
            return _handlers[scheme].GetCredentials(token);
        }
    }
}