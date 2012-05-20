using System;
using System.Collections;
using System.Web;

namespace Teamworks.Core.Services
{
    public static class Local
    {
        private static readonly ILocalData _data = new LocalData();

        public static ILocalData Data
        {
            get { return _data; }
        }

        #region Nested type: LocalData

        private class LocalData : ILocalData
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
                        var web_hashtable = HttpContext.Current.Items[LocalDataHashtableKey] as Hashtable;
                        if (web_hashtable == null)
                        {
                            web_hashtable = new Hashtable();
                            HttpContext.Current.Items[LocalDataHashtableKey] = web_hashtable;
                        }
                        return web_hashtable;
                    }
                }
            }

            private static bool RunningInWeb
            {
                get { return HttpContext.Current != null; }
            }

            #region ILocalData Members

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