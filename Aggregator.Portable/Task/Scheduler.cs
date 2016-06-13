using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Exceptions;

namespace Aggregator.Task
{
    public class Scheduler : IDisposable
    {
        private readonly List<RepeatingTask> _tasks;

        private bool _isStarted;

        public event EventHandler<Exception> TaskException;

        public Scheduler()
        {
            _tasks = new List<RepeatingTask>();
        }

        public virtual void AddTask(
            Guid taskId,
            ITask task,
            int pollingInterval,
            bool isActive)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            var scheduledTask = new RepeatingTask(
                taskId,
                task,
                TimeSpan.FromMilliseconds(pollingInterval), 
                isActive,
                ex =>
                {
                    var handler = TaskException;
                    handler?.Invoke(this, ex);
                });

            _tasks.Add(scheduledTask);
        }

        public void UpdatePollingInterval(
            Guid taskId, 
            int newPollingInterval)
        {
            var task = _tasks.Single(t => t.TaskId == taskId);

            task.Cancel();
            task.ChangeRepetitionPeriod(
                TimeSpan.FromMilliseconds(newPollingInterval));

            if (_isStarted)
            {
                task.Start();
            }
        }

        public void SuspendTask(Guid taskId)
        {
            var task = _tasks.Single(t => t.TaskId == taskId);
            task.Deactivate();
        }

        public void ResumeTask(Guid taskId)
        {
            var task = _tasks.Single(t => t.TaskId == taskId);
            task.Activate();
            if (_isStarted)
            {
                task.Start();
            }
        }

        public virtual void Start()
        {
            if (_isStarted)
            {
                throw new TaskSchedulerStateException();
            }

            _isStarted = true;

            foreach (var task in _tasks)
            {
                task.Start();
            }
        }

        public virtual void Stop()
        {
            if (!_isStarted)
            {
                throw new TaskSchedulerStateException();
            }

            StopAllWorkers();

            _isStarted = false;
        }

        public virtual void Force()
        {
            if (_isStarted)
            {
                throw new TaskSchedulerStateException();
            }

            _isStarted = true;

            var taskList = _tasks
                .Select(task => task.Force())
                .ToArray();

            try
            {
                System.Threading.Tasks.Task.WaitAll(taskList);
            }
            catch (AggregateException e)
            {
                if (TaskException != null)
                {
                    foreach (var exception in e.InnerExceptions)
                    {
                        TaskException(this, exception);
                    }
                }
            }

            _isStarted = false;
        }

        private void StopAllWorkers()
        {
            foreach (var task in _tasks)
            {
                task.Cancel();
            }
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_isStarted)
                    {
                        Stop();
                        _tasks.Clear();
                        TaskException = null;
                    }
                }

                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    public class RepeatingTask
    {
        private readonly ITask _repeatingTask;
        private readonly Action<Exception> _exceptionHandler;

        private bool _isActive;
        private TimeSpan _repetitionPeriod;
        private CancellationTokenSource _tokenSource;
        private DateTime _lastRun;

        public Guid TaskId { get; private set; }

        public RepeatingTask(
            Guid taskId,
            ITask repeatingTask,
            TimeSpan repetitionPeriod,
            bool isActive,
            Action<Exception> exceptionHandler)
        {
            _repeatingTask = repeatingTask;
            _repetitionPeriod = repetitionPeriod;
            _isActive = isActive;
            _exceptionHandler = exceptionHandler;
            _lastRun = DateTime.MinValue;
            TaskId = taskId;
        }

        public void Start()
        {
            if (_isActive)
            {
                _tokenSource = new CancellationTokenSource();
                Run();
            }
        }

        public System.Threading.Tasks.Task Force()
        {
            if (_isActive)
            {
                return RunOnce(
                    () => _repeatingTask.Execute(),
                    _repetitionPeriod);
            }

            return System.Threading.Tasks.Task.FromResult(new object());
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
            Cancel();
        }

        public void ChangeRepetitionPeriod(TimeSpan newRepetitionPeriod)
        {
            _repetitionPeriod = newRepetitionPeriod;
        }

        public void Cancel()
        {
            _tokenSource?.Cancel();
            _tokenSource = null;
        }

        private async System.Threading.Tasks.Task RunForever(
            Action action,
            TimeSpan period,
            CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                action();
                _lastRun = DateTime.Now;
                await System.Threading.Tasks.Task.Delay(period, cancellationToken);
            }
        }

        private async System.Threading.Tasks.Task RunOnce(
            Action action,
            TimeSpan period)
        {
            var lag = DateTime.Now - _lastRun;

            if (lag > period)
            {
                await System.Threading.Tasks.Task.Run(action);

            }
        }

        private System.Threading.Tasks.Task Run()
        {
            var result = RunForever(
                () => _repeatingTask.Execute(),
                _repetitionPeriod,
                _tokenSource.Token);
            result.ConfigureAwait(false);
            result.ContinueWith(
                t => _exceptionHandler(t.Exception),
                TaskContinuationOptions.OnlyOnFaulted)
                .ConfigureAwait(false);
            return result;
        }
    }
}
