using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Aggregator.Configuration;
using Aggregator.Configuration.Startup;
using Aggregator.Domain.Read;
using Aggregator.Domain.Write;
using Aggregator.Task;
using Aggregator.Task.Commands;
using Aggregator.Test.Helpers;
using Aggregator.Test.Helpers.Mocks;
using Aggregator.Test.Mocks;
using Xunit;

namespace Aggregator.Test.Unit.Tasks
{
    public class PopulateSchedulerTaskTests
    {

        [Fact]
        public void ExecuteSchedulerTaskWithUserButNoSensorsAddsNoTasksToScheduler()
        {
            var scheduler = new Scheduler();
            var database = new GenericDatabaseMock();
            var readModel = new ReadModelFacade(database);
            var factory = new SampleRequestTaskFactoryMock();
            var commandSender = new CommandSenderMock();
            var personId = Guid.NewGuid();
            UserSettings.UserId = personId;

            var task = new PopulateSchedulerStartupTask(
                scheduler,
                readModel,
                factory,
                commandSender);

            task.Execute();

            var scheduledTasks = scheduler.GetMemberValue<List<RepeatingTask>>("_tasks");

            Assert.Equal(0, scheduledTasks.Count);
        }

        [Fact]
        public void ExecuteSchedulerTaskWithUserAndSensorsAddsTasksToScheduler()
        {
            var scheduler = new Scheduler();
            var databaseMock = new GenericDatabaseMock();
            var readModel = new ReadModelFacade(databaseMock);
            var factory = new SampleRequestTaskFactoryMock();
            var commandSender = new CommandSenderMock();
            var personId = Guid.NewGuid();
            UserSettings.UserId = personId;
            var sensor = new SensorDto(
               Guid.NewGuid(),
               personId,
               TimeSpan.FromSeconds(1), 
               new JObject(),
               typeof(ResourceProviderMock),
               typeof(RequestMock),
               null,
               0);
            databaseMock.Put(sensor);

            var task = new PopulateSchedulerStartupTask(
                scheduler,
                readModel,
                factory,
                commandSender);

            task.Execute();

            var scheduledTasks = scheduler.GetMemberValue<List<RepeatingTask>>("_tasks");

            Assert.Equal(1, scheduledTasks.Count);
        }

        [Fact]
        public void ExecuteScheudlerTaskWithNoUserAddsUser()
        {
            UserSettings.UserId = Guid.Empty;
            var commandSender = new CommandSenderMock();

            var task = new PopulateSchedulerStartupTask(
                null,
                null,
                null,
                commandSender);

            task.Execute();

            var lastCommand = commandSender.GetLastCommand<CreatePersonCommand>();

            Assert.NotNull(lastCommand);
        }
    }
}
