using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Teamworks.Core.Services.Executor
{
    public class Executor
    {
        #region Singleton

        private static Executor _instance;

        public static Executor Instance
        {
            get { return _instance ?? (_instance = new Executor()); }
        }

        #endregion

        #region Private Properties 

        private readonly AutoResetEvent ARE;
        private readonly Stack<Task>[] queue;
        private volatile bool run;

        private Stack<Task> High
        {
            get { return queue[(int) ExecutePriority.HIGH]; }
            set { queue[(int) ExecutePriority.HIGH] = value; }
        }

        private Stack<Task> Medium
        {
            get { return queue[(int) ExecutePriority.MEDIUM]; }
            set { queue[(int) ExecutePriority.MEDIUM] = value; }
        }

        private Stack<Task> Low
        {
            get { return queue[(int) ExecutePriority.LOW]; }
            set { queue[(int) ExecutePriority.LOW] = value; }
        }

        #endregion

        private Executor()
        {
            ARE = new AutoResetEvent(true);

            queue = new Stack<Task>[3];
            High = new Lazy<Stack<Task>>(() => new Stack<Task>()).Value;
            Medium = new Lazy<Stack<Task>>(() => new Stack<Task>()).Value;
            Low = new Lazy<Stack<Task>>(() => new Stack<Task>()).Value;

            Timeout = 60000;
        }

        #region Private Methods 

        private Task Next()
        {
            if (High.Count > 0)
                return High.Pop();

            if (Medium.Count > 0)
                return Medium.Pop();

            if (Low.Count > 0)
                return Low.Pop();

            return null;
        }

        private bool HasTask()
        {
            return High.Count > 0 || Medium.Count > 0 || Low.Count > 0;
        }

        #endregion

        public int Timeout { get; set; }

        public Task Enqueue(Action action, ExecutePriority priority = ExecutePriority.MEDIUM)
        {
            var task = new Task(action);
            queue[(int) priority].Push(task);
            ARE.Set();
            return task;
        }

        public void Initialize()
        {
            run = true;
            Task.Factory.StartNew(
                () =>
                    {
                        while (run)
                        {
                            if (HasTask())
                            {
                                Next().Start();
                                ARE.Reset();
                            }
                            else
                                ARE.WaitOne(Timeout);
                        }
                    }
                );
        }

        public void Stop()
        {
            run = false;
            ARE.Set();
        }
    }

    public enum ExecutePriority
    {
        HIGH = 2,
        MEDIUM = 1,
        LOW = 0
    }
}