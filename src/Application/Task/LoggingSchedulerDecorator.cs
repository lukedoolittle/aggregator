using System;
using Aggregator.Framework.Contracts;

namespace Aggregator.Task
{
    public class LoggingSchedulerDecorator : Scheduler
    {
        private readonly ILogger _logger;

        public LoggingSchedulerDecorator(ILogger logger)
        {
            _logger = logger;

            base.TaskException += 
                (sender, exception) => 
                    _logger.Error(exception.Message);
        }

        public override void AddTask(
            Guid taskId, 
            ITask task, 
            TimeSpan pollingInterval, 
            bool isActive)
        {
            _logger.Info("Task added to scheduler");
            base.AddTask(taskId, task, pollingInterval, isActive);
        }

        public override void Start()
        {
            _logger.Info("Scheduler started");
            base.Start();
        }

        public override void Stop()
        {
            _logger.Info("Scheduler stopped");
            base.Stop();
        }

        public override void Force()
        {
            _logger.Info("Forced run on scheduler");
            base.Force();
        }
    }
}
