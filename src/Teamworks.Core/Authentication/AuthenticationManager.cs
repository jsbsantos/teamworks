using System;
using System.Collections.Generic;
using System.Net;

namespace Teamworks.Core.Authentication {
    public static class AuthenticationManager {
        private static string _defaultScheme;

        private static readonly Dictionary<string, IAuthenticationHandler> Handlers;

        static AuthenticationManager() {
            Handlers = new Dictionary<string, IAuthenticationHandler>();
        }

        public static string DefaultAuthenticationScheme {
            get { return _defaultScheme; }
            set {
                if (Handlers.ContainsKey(value)) {
                    _defaultScheme = value;
                }
                else {
                    throw new ArgumentException("Scheme");
                }
            }
        }

        public static bool Validate(string scheme, NetworkCredential credential) {
            if (!Handlers.ContainsKey(scheme)) {
                throw new ArgumentException("scheme");
            }
            return Handlers[scheme].IsValid(credential);
        }

        public static void Add(string scheme, IAuthenticationHandler handler) {
            Handlers.Add(scheme, handler);
        }

        public static void Remove(string scheme) {
            Handlers.Remove(scheme);
        }

        public static IEnumerable<string> GetRegisteredSchemes() {
            return Handlers.Keys;
        }

        public static NetworkCredential GetCredentials(string scheme, dynamic token) {
            if (!Handlers.ContainsKey(scheme)) {
                throw new ArgumentException("scheme");
            }
            return Handlers[scheme].GetCredentials(token);
        }

        public static IAuthenticationHandler Get(string scheme) {
            IAuthenticationHandler value;
            if (Handlers.TryGetValue(scheme, out value)) {
                return value;
            }
            return null;

        }
    }
}