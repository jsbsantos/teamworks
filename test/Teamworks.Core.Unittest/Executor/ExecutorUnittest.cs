using System;
using System.Threading;
using System.Threading.Tasks;
using Teamworks.Core.Services;
using Xunit;

namespace Teamworks.Core.Unittest.Executor
{
    public class ExecutorUnittest
    {
        [Fact]
        public void ForgeTaskPriorityExecution()
        {
            var executor = Teamworks.Core.Services.Executor.Instance;
            executor.Timeout = 2000;
            int r = 0, o1 = 0, o2 = 0, o3 = 0;

            var t1 = executor.Enqueue(() =>
                                          {
                                              var local = Interlocked.Increment(ref r);
                                              o3 = local;
                                          }, ExecutePriority.LOW).ContinueWith(
                                              a => { Assert.False(a.IsFaulted); });
            var t2 = executor.Enqueue(() =>
                                          {
                                              var local = Interlocked.Increment(ref r);
                                              o2 = local;
                                          }, ExecutePriority.MEDIUM).ContinueWith(
                                              a => { Assert.False(a.IsFaulted); });

            var t3 = executor.Enqueue(() =>
                                          {
                                              var local = Interlocked.Increment(ref r);
                                              o1 = local;
                                          }, ExecutePriority.HIGH).ContinueWith(
                                              a => { Assert.False(a.IsFaulted); });

            executor.Initialize();

            t1.Wait();
            t2.Wait();
            t3.Wait();

            Assert.True(o1 == 1 && o2 == 2 && o3 == 3);
        }

        [Fact]
        public void ForgeTaskPriorityExecution_With_Sleep()
        {
            var executor = Teamworks.Core.Services.Executor.Instance;
            executor.Initialize();
            executor.Timeout = 2000;
            int r = 0, o1 = 0, o2 = 0, o3 = 0;

            var t1 = executor.Enqueue(() => { Thread.Sleep(2000); }, ExecutePriority.LOW).ContinueWith(
                a =>
                    {
                        Assert.False(a.IsFaulted);
                        Interlocked.Increment(ref r);
                    });

            Thread.Sleep(10000);

            var t2 = executor.Enqueue(() => { Thread.Sleep(3000); }, ExecutePriority.MEDIUM).ContinueWith(
                a =>
                    {
                        Assert.False(a.IsFaulted);
                        Interlocked.Increment(ref r);
                    });

            t1.Wait();
            t2.Wait();

            Assert.True(r == 2);
        }
    }
}