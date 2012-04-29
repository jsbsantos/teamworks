using System;
using Newtonsoft.Json;
using Raven.Client;
using Raven.Client.Linq;
using Teamworks.Core.Extensions;
using Teamworks.Core.People;

namespace Teamworks.Core {
    public abstract class Entity<T> : BaseEntity<T> {
        #region Implementation of IEntity

        public string Id { get; set; }
        public string Name { get; set; }

        #endregion

        protected Entity() {
            CreatedAt = UpdatedAt = DateTime.Now;
        }

        [JsonIgnore]
        public int Identifier {
            get {
                int i;
                if (string.IsNullOrEmpty(Id) || (i = Id.IndexOf('/')) < 0) {
                    return 0;
                }

                int id;
                return int.TryParse(Id.Substring(i + 1, Id.Length - i - 1), out id) ? id : 0;
            }
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Reference<Person> Creator { get; set; }
    }

    #region Abstract

    public abstract class AuditableEntity {
        private DateTime? _createdAt;
        private DateTime? _updatedAt;

        public DateTime CreatedAt {
            set {
                if (!_createdAt.HasValue) {
                    _createdAt = value;
                }
            }
            get {
                if (!_createdAt.HasValue) {
                    _createdAt = DateTime.Now;
                }
                return _createdAt.Value;
            }
        }

        public DateTime UpdatedAt {
            set {
                if (!_updatedAt.HasValue) {
                    _updatedAt = value;
                }
            }
            get { return DateTime.Now; }
        }

        public string CreatedBy { set; get; }
        public string UpdatedBy { set; get; }
    }

    public abstract class BaseEntity<T> {
        protected static IDocumentSession Session {
            get { return ((IDocumentSession) Local.Data[Global.RavenSessionkey]); }
        }

        #region CRD

        public static T Get(string id) {
            return Session.Load<T>(id);
        }

        public static T Add(T entity) {
            Session.Store(entity);
            return entity;
        }

        public static void Remove(T entity) {
            Session.Delete(entity);
        }

        public static IRavenQueryable<T> Query() {
            return Session.Query<T>();
        }

        #endregion
    }

    #endregion
}