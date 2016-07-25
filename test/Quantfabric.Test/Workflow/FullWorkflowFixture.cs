using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Aggregator.Domain.Events;
using Aggregator.Test.Helpers;
using Material.Infrastructure.Credentials;
using Material.Infrastructure.ProtectedResources;
using Material.Infrastructure.Requests;
using SimpleCQRS.Domain;
using SimpleCQRS.Framework.Contracts;
using Xunit;
using TwitterTweet = Material.Infrastructure.Requests.TwitterTweet;


namespace Aggregator.Test.Interaction
{
    public class FullWorkflowFixture
    {
        [Fact]
        public async void BootstrapAndRunStopSchedulerAndAddNewServiceThenResumeRunning()
        {
            var expectedExceptionCount = 0;
            var minimumNumberOfSamples = 20;

            using (var helper = new WorkflowFixture())
            {
                //Create a new Person
                var personId = Guid.NewGuid();
                helper.User.CreatePerson(personId);

                //Stop the scheduler while creating a service (a good idea)
                helper.Scheduler.StopScheduler();

                //Add a service token and a sensor
                await helper.User.CreateToken<Google>();
                helper.User.CreateSensor<GoogleGmail, Google>(TimeSpan.FromSeconds(1));

                //Restart the scheduler to enable polling, wait for a bit and then stop
                await helper.Scheduler.PulseOn(10000);

                //Assert valid data here
                helper.User.AssertPersonIdIsAppSettingPersonId();
                var exceptions = helper.Scheduler.TaskExceptionCount();
                Assert.Equal(expectedExceptionCount, exceptions);

                //Assert tokens and sensors exist in write repository
                var write = helper.User.WriteRepositoryRepresentation<GoogleGmail, Google>();
                Assert.NotNull(write.Token);
                Assert.NotNull(write.Sensor);

                //Create this dummy event to get a query id
                var sensor = helper.User.SensorReadRepositoryRepresentation<GoogleGmail>();
                Assert.NotNull(sensor);

                //doesn't check other sensor parameters such as polling interval
                Assert.Equal(typeof (Google).Name, sensor.ServiceType.Name);
                Assert.Equal(true, sensor.IsActive);
                Assert.Equal(typeof (GoogleGmail).Name, sensor.SensorType.Name);

                var samples = helper.User.SampleReadRepositoryRepresentation<GoogleGmail>();
                Assert.True(samples.Count() >= minimumNumberOfSamples);
                foreach (var sample in samples)
                {
                    Assert.NotNull(sample);
                }

                AssertLastQueryIsCorrect();
            }
        }

        [Fact]
        public async void BootstrapWithNoRunValidateAServiceAndThenRevalidateExpectedAnUpdatedToken()
        {
            using (var helper = new WorkflowFixture())
            {
                //Create a new Person
                var personId = Guid.NewGuid();
                helper.User.CreatePerson(personId);

                //Stop the scheduler while creating a service (a good idea)
                helper.Scheduler.StopScheduler();

                //Add a service token and a sensor
                await helper.User.CreateToken<Facebook>();
                helper.User.CreateSensor<FacebookFeed, Facebook>(TimeSpan.FromSeconds(1));

                var token1 = helper.Scheduler.GetToken<FacebookFeed, OAuth2Credentials, Facebook>();

                await System.Threading.Tasks.Task.Delay(3000);

                //Create another token for an already authorized service
                await helper.User.CreateToken<Facebook>();

                var token2 = helper.Scheduler.GetToken<FacebookFeed, OAuth2Credentials, Facebook>();

                Assert.NotNull(token1);
                Assert.NotNull(token2);
                Assert.NotEqual(token1.DateCreated, token2.DateCreated);
            }

            //Teardown the service and then rerun the bootstrap and see if we get the same token back
            using (var helper = new WorkflowFixture())
            {
                var token3 = helper.Scheduler.GetToken<FacebookFeed, OAuth2Credentials, Facebook>();
                Assert.NotNull(token3);

                AssertLastQueryIsCorrect();
            }
        }

        private void AssertLastQueryIsCorrect()
        {
            //here we should dig through taskscheduler->repeatingtasks->itask->lastquery
            //and grab the last query value and compare to expected???
        }

        [Fact]
        public async void BootstrapAndRunValidateServiceAndThenDeactivateAndReactivateSensor()
        {
            int exitSampleCount = 0;

            using (var helper = new WorkflowFixture())
            {
                var user = helper.User;

                var personId = Guid.NewGuid();
                user.CreatePerson(personId);

                //Stop the scheduler while creating a service (a good idea)
                helper.Scheduler.StopScheduler();

                await user.CreateToken<Twitter>();
                user.CreateSensor<TwitterTweet, Twitter>(TimeSpan.FromSeconds(1));

                //Restart the scheduler to enable polling, wait for a bit and then stop
                await helper.Scheduler.PulseOn(10000);

                user.DeactivateASensor<TwitterTweet, Twitter>();

                var initialSamples = user.SampleReadRepositoryRepresentation<TwitterTweet>().Count();

                //Restart the scheduler to enable polling, wait for a bit and then stop
                await helper.Scheduler.PulseOn(10000);

                user.ActivateASensor<TwitterTweet, Twitter>();

                //Restart the scheduler to enable polling, wait for a bit and then stop
                await helper.Scheduler.PulseOn(10000);

                var finalSamples = user.SampleReadRepositoryRepresentation<TwitterTweet>().Count();

                Assert.True(finalSamples >= initialSamples);

                AssertLastQueryIsCorrect();

                exitSampleCount = finalSamples;
                helper.User.RetainUserOnDispose();
            }

            //Teardown the service and restart to check that duplicate removal is working effectively
            using (var helper = new WorkflowFixture())
            {
                await System.Threading.Tasks.Task.Delay(1000);

                var samplesAfterTeardown = helper.User.SampleReadRepositoryRepresentation<TwitterTweet>().Count();

                Assert.Equal(exitSampleCount, samplesAfterTeardown);
            }
        }

        //[Fact]
        //public async void EndToEndCreationOfMultipleSensorsAndEncodingOfValues()
        //{
        //    using (var helper = new WorkflowFixture())
        //    {
        //        var encodings = new ConcurrentBag<Event>();
        //        var user = helper.User;

        //        var personId = Guid.NewGuid();
        //        user.CreatePerson(personId);

        //        //Stop the scheduler while creating a service (a good idea)
        //        helper.Scheduler.StopScheduler();

        //        await user.CreateToken<Twitter>();
        //        user.CreateSensor<TwitterTweet, Twitter>(TimeSpan.FromSeconds(1));
        //        await helper.User.CreateToken<Facebook>();
        //        helper.User.CreateSensor<FacebookFeed, Facebook>(TimeSpan.FromSeconds(1));
        //        await helper.User.CreateToken<Google>();
        //        helper.User.CreateSensor<GoogleGmail, Google>(TimeSpan.FromSeconds(1));
        //        await helper.User.CreateToken<Rescuetime>();
        //        helper.User.CreateSensor<RescuetimeAnalyticData, Rescuetime>(TimeSpan.FromSeconds(1));
        //        await helper.User.CreateToken<Fitbit>();
        //        helper.User.CreateSensor<FitbitIntradaySteps, Fitbit>(TimeSpan.FromSeconds(1));
        //        helper.User.CreateSensor<FitbitSleep, Fitbit>(TimeSpan.FromSeconds(1));

        //        var manager = ServiceLocator.Current.GetInstance<ISubscriptionManager>();

        //        manager.Subscribe((EncodingCreated<Mood> encoding) => encodings.Add(encoding));
        //        manager.Subscribe((EncodingCreated<Productivity> encoding) => encodings.Add(encoding));
        //        manager.Subscribe((EncodingCreated<Activity> encoding) => encodings.Add(encoding));
        //        manager.Subscribe((EncodingCreated<Rest> encoding) => encodings.Add(encoding));

        //        //Restart the scheduler to enable polling, wait for a bit and then stop
        //        await helper.Scheduler.PulseOn(10000);

        //        Assert.True(encodings.Any(e=> e is EncodingCreated<Mood>));
        //        Assert.True(encodings.Any(e => e is EncodingCreated<Productivity>));
        //        Assert.True(encodings.Any(e => e is EncodingCreated<Activity>));
        //        Assert.True(encodings.Any(e => e is EncodingCreated<Rest>));
        //    }
        //}
    }
}
