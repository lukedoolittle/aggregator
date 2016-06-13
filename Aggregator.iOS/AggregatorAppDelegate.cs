using System;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using Aggregator.Configuration;
using Aggregator.Framework.Contracts;
using Aggregator.Task;
using UIKit;

namespace Aggregator
{

    public class AggregatorAppDelegate : UIApplicationDelegate
    {
        private Scheduler _scheduler;
        private bool _isSchedulerActive;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            _scheduler = new Bootstrapper().Run().GetInstance<Scheduler>();

            //At the mercy of iOS we request that the background manager try and fetch
            //our data as frequently as possible
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(
                UIApplication.BackgroundFetchIntervalMinimum);

            return true;
        }

        public override void OnActivated(UIApplication application)
        {
            if (!_isSchedulerActive)
            {
                _scheduler.Start();
                _isSchedulerActive = true;
            }
        }

        public override void PerformFetch(
            UIApplication application, 
            Action<UIBackgroundFetchResult> completionHandler)
        {
            //Force the scheduler to run a single time; any tasks that are
            //not due to run will not execute
            _scheduler.Force();

            completionHandler(UIBackgroundFetchResult.NewData);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            //This prevents iOS from killing our app; if we let the scheduler
            //keep running the watchdog will kill the app
            if (_isSchedulerActive)
            {
                _scheduler.Stop();
                _isSchedulerActive = false;
            }
        }
    }

    public class LoggingAggregatorAppDelegate : AggregatorAppDelegate
    {
        private ILogger _logger;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            base.FinishedLaunching(app, options);

            _logger = ServiceLocator.Current.GetInstance<ILogger>();
            _logger.Info("Finished launching application");

            return true;
        }

        public override void OnActivated(UIApplication application)
        {
            _logger.Info("Application activated");
            base.OnActivated(application);
        }

        public override void PerformFetch(
            UIApplication application,
            Action<UIBackgroundFetchResult> completionHandler)
        {
            _logger.Info("Performing application fetch");
            base.PerformFetch(application, completionHandler);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            _logger.Info("Application entering background");
            base.DidEnterBackground(application);
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.

            _logger.Info("Application moving to inactive state");
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.

            _logger.Info("Application entering foreground");
            base.WillEnterForeground(application);
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.

            _logger.Info("Application terminating");
            base.WillTerminate(application);
        }
    }
}