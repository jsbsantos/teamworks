using System;
using System.Collections;
using System.Web;

namespace Teamworks.Core.Services.Storage
{
    public static class Local
    {
        private static readonly IStorage _data = new Storage();

        public static IStorage Data
        {
            get { return _data; }
        }

        #region Nested type: Storage

        private class Storage : IStorage
        {
            [ThreadStatic] private static Hashtable _localData;
            private static readonly object LocalDataHashtableKey = new object();

            private static Hashtable LocalHashtable
            {
                get
                {
                    if (!RunningInWeb)
                    {
                        return _localData ?? (_localData = new Hashtable());
                    }
                    else
                    {
                        var webHashtable = HttpContext.Current.Items[LocalDataHashtableKey] as Hashtable;
                        if (webHashtable == null)
                        {
                            webHashtable = new Hashtable();
                            HttpContext.Current.Items[LocalDataHashtableKey] = webHashtable;
                        }
                        return webHashtable;
                    }
                }
            }

            private static bool RunningInWeb
            {
                get { return HttpContext.Current != null; }
            }

            #region IStorage Members

            public object this[object key]
            {
                get { return LocalHashtable[key]; }
                set { LocalHashtable[key] = value; }
            }

            public int Count
            {
                get { return LocalHashtable.Count; }
            }

            public void Clear()
            {
                LocalHashtable.Clear();
            }

            #endregion
        }

        #endregion
    }
}