using System;
using System.Reflection;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Microsoft.Practices.ServiceLocation;
using Aggregator.Configuration;
using Aggregator.Framework.Contracts;
using Aggregator.Task;
using Aggregator.Task.Factories;
using SimpleCQRS.Infrastructure;

namespace Aggregator
{
    [Service]
    public class AggregatorService : Service
    {
        public static System.Threading.Tasks.Task Initializing =>
            _isInitializingCompletionSource.Task;
        private static readonly TaskCompletionSource<bool> _isInitializingCompletionSource = 
            new TaskCompletionSource<bool>();
        private Scheduler _scheduler;

        public override void OnCreate()
        {
            _scheduler = new Bootstrapper()
                .DefaultConfig()
                .Run()
                .GetInstance<Scheduler>();
            _isInitializingCompletionSource.SetResult(true);

            base.OnCreate();
        }

        public override StartCommandResult OnStartCommand(
            Intent intent, 
            StartCommandFlags flags, 
            int startId)
        {
            _scheduler.Start();

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            _scheduler.Stop();

            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
    }

    [Service]
    public class LoggingAggregatorService : AggregatorService
    {
        private ILogger _logger;

        public override void OnCreate()
        {
            base.OnCreate();

            _logger = ServiceLocator.Current.GetInstance<ILogger>();

            _logger.Info("Aggregator service initialized");
        }

        public override StartCommandResult OnStartCommand(
            Intent intent,
            StartCommandFlags flags,
            int startId)
        {
            _logger.Info("Aggregator service initialized");

            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnDestroy()
        {
            _logger.Info("Aggregator service stopped");

            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            _logger.Error("Binding to Aggregator Service");

            throw new NotImplementedException();
        }
    }
}