using System;
using Aggregator.Configuration;
using Aggregator.Task;

namespace Aggregator.Test.Helpers
{
    public class WorkflowFixture : IDisposable
    {
        public TaskSchedulerFixture Scheduler { get; }
        public UserFixture User { get; }

        public WorkflowFixture()
        {
            //TODO: this may actually need to be the platform bootstrapper
            var resolver = new Bootstrapper().Run();

            resolver.GetInstance<Scheduler>().Start();

            User = resolver.GetInstance<UserFixture>();
            Scheduler = resolver.GetInstance<TaskSchedulerFixture>();
        }

        public void Dispose()
        {
            Scheduler.Dispose();
            User.Dispose();
        }
    }
}
