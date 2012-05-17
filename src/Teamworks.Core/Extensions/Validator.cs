﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Teamworks.Core.Extensions
{
    public class Validator
    {
        private static Validator _instance;
        private static Dictionary<string, Func<bool>> _validators;
        private int timeout;

        private Validator()
        {
            _validators = new Dictionary<string, Func<bool>>();
            timeout = 100;
        }

        public static Validator Instance
        {
            get { return _instance ?? (_instance = new Validator()); }
        }

        public bool Validate(Type type)
        {
            string name = type.Name;
            if (!_validators.ContainsKey(name))
            {
                return true;
            }

            //return ValidateWithThreadPool(type);
            //return ValidateWithTPLNoTimeout(type);
            return ValidateWithTPLWithTimeout(type);
        }

        private bool ValidateWithTPLNoTimeout(Type type)
        {
            string name = type.Name;
            Func<bool> validators;
            lock (_validators[name])
            {
                validators = _validators[name].Clone() as Func<bool>;
            }
            Delegate[] vld = validators.GetInvocationList();
            bool res = true;

            Parallel.ForEach(vld, x => res &= ((Func<bool>) x).Invoke());

            return res;
        }

        private bool ValidateWithTPLWithTimeout(Type type)
        {
            string name = type.Name;
            Func<bool> validators;
            lock (_validators[name])
            {
                validators = _validators[name].Clone() as Func<bool>;
            }
            IEnumerable<Func<bool>> vld = validators.GetInvocationList().Cast<Func<bool>>();
            bool res = true;

            var cts = new CancellationTokenSource();
            var t = new Timer(_ => cts.Cancel(), null, timeout, -1);
            try
            {
                Parallel.ForEach(
                    vld,
                    new ParallelOptions {CancellationToken = cts.Token},
                    x => res &= x.Invoke());
            }
            catch (OperationCanceledException oce)
            {
                throw new TimeoutException("Validation Timeout Exceeded");
            }

            return res;
        }

        private bool ValidateWithThreadPool(Type type)
        {
            string name = type.Name;
            if (!_validators.ContainsKey(name))
            {
                return true;
            }

            Func<bool> validators;
            lock (_validators[name])
            {
                validators = _validators[name].Clone() as Func<bool>;
            }
            Delegate[] vld = validators.GetInvocationList();
            bool res = true;

            List<IAsyncResult> asyncResults = (from Func<bool> v in vld
                                               select v.BeginInvoke(x => res &= v.EndInvoke(x), null))
                .ToList();

            if (!WaitHandle.WaitAll(asyncResults.Select(v => v.AsyncWaitHandle).ToArray(), timeout))
            {
                throw new TimeoutException("Validation Timeout Exceeded");
            }

            return res;
        }

        public Validator Register(Type type, Func<bool> func)
        {
            string name = type.Name;
            if (!_validators.ContainsKey(name))
            {
                _validators.Add(name, () => true);
            }
            _validators[name] += func;
            return this;
        }

        public Validator Unregister(Type type, Func<bool> func)
        {
            string name = type.Name;
            if (_validators.ContainsKey(name))
            {
                _validators[name] -= func;
            }
            return this;
        }

        public Validator SetTimeout(int timeoutInMillis)
        {
            timeout = timeoutInMillis;
            return this;
        }
    }
}