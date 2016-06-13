using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Aggregator.Domain.Write;
using Aggregator.Framework.Contracts;
using Aggregator.Infrastructure.Credentials;
using Aggregator.Task;
using Aggregator.Task.Requests;

namespace Aggregator.Test.Helpers
{
    public class TaskSchedulerFixture : IDisposable
    {
        private readonly Scheduler _scheduler;
        private readonly ConcurrentBag<Exception> _exceptions;

        public IEnumerable<Exception> Exceptions => _exceptions;

        public TaskSchedulerFixture(Scheduler scheduler)
        {
            _scheduler = scheduler;
            
            _exceptions = new ConcurrentBag<Exception>();

            _scheduler.TaskException += (sender, exception) => _exceptions.Add(exception);
        }

        public async System.Threading.Tasks.Task PulseOn(int milliseconds)
        {
            _scheduler.Start();
            await System.Threading.Tasks.Task.Delay(milliseconds);
            _scheduler.Stop();
        }

        public void StopScheduler()
        {
            _scheduler.Stop();
        }

        public int TaskExceptionCount()
        {
            return _exceptions.Count;
        }

        public TCredentials GetToken<TRequest, TCredentials, TService>()
            where TRequest : Request, new()
            where TService : Service
            where TCredentials : TokenCredentials
        {
            var tasks = _scheduler.GetMemberValue<List<RepeatingTask>>("_tasks");
            var repeatingTask = tasks.SingleOrDefault(
                t => 
                    t.GetMemberValue<ITask>("_repeatingTask") is SampleRequestTask<TRequest, TCredentials, TService>);
            var task = repeatingTask.GetMemberValue<SampleRequestTask<TRequest, TCredentials, TService>>("_repeatingTask");

            return task.GetMemberValue<TCredentials>("_credentials");
        }
        
        public void Dispose()
        {
            _scheduler.Dispose();
        }
    }
}
