using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teamworks.Core.Extensions
{
    public class Validator
    {
        private static Validator _instance;
        private static Dictionary<string, Func<bool>> _validators;
        private int timeout;

        public static Validator Instance
        {
            get
            {
                return _instance ?? (_instance = new Validator());
            }
        }

        private Validator()
        {
            _validators = new Dictionary<string, Func<bool>>();
            timeout = 3600000;
        }

        public bool Validate(Type type)
        {
            var name = type.Name;
            if (!_validators.ContainsKey(name))
                return true;

            Func<bool> validators;
            lock (_validators[name])
            {
                validators = _validators[name].Clone() as Func<bool>;
            }
            var vld = validators.GetInvocationList();
            var res = true;

            Parallel.ForEach(vld, x => res &= ((Func<bool>)x).Invoke());

            //var asyncResults = (from Func<bool> v in vld select v.BeginInvoke(x => res &= v.EndInvoke(x), null)).ToList();

            /*if (!WaitHandle.WaitAll(asyncResults.Select(v => v.AsyncWaitHandle).ToArray(), 3600000))
                throw new TimeoutException("Validation Timeout Exceeded");*/

            return res;
        }

        public Validator Register(Type type, Func<bool> func)
        {
            var name = type.Name;
            if (!_validators.ContainsKey(name))
                _validators.Add(name, () => true);
            _validators[name] += func;
            return this;
        }

        public Validator Unregister(Type type, Func<bool> func)
        {
            var name = type.Name;
            if (_validators.ContainsKey(name))
                _validators[name] -= func;
            return this;
        }

        public Validator SetTimeout(int timeoutInMillis)
        {
            timeout = timeoutInMillis;
            return this;
        }

    }
}