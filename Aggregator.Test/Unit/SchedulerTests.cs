using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aggregator.Framework.Exceptions;
using Aggregator.Task;
using Aggregator.Test.Helpers;
using Aggregator.Test.Helpers.Mocks;
using Xunit;

namespace Aggregator.Test.Unit
{
    
    public class SchedulerTests
    {
        [Fact]
        public void StartWithEmptyQueueDoesntThrowException()
        {
            var actual = new Scheduler();
            actual.Start();
        }

        [Fact]
        public void PauseWithEmptyQueueDoesntThrowException()
        {
            var actual = new Scheduler();
            actual.Start();
        }

        [Fact]
        public void DisposingWithEmptyQueueDoesntThrowException()
        {
            var actual = new Scheduler();
            actual.Start();
        }

        [Fact]
        public void StartingTaskQueueWhileActiveThrowsException()
        {
            var actual = new Scheduler();

            actual.Start();
            Assert.Throws<TaskSchedulerStateException>(()=>actual.Start());
        }

        [Fact]
        public void PausingTaskQueueWhileInactiveThrowsException()
        {
            var actual = new Scheduler();

            actual.Start();
            actual.Stop();
            Assert.Throws<TaskSchedulerStateException>(() => actual.Stop());
        }

        [Fact]
        public void PausingTaskQueueBeforeStartingThrowsException()
        {
            var actual = new Scheduler();

            Assert.Throws<TaskSchedulerStateException>(() => actual.Stop());
        }

        [Fact]
        public void AddingANullTaskShouldThrowAnException()
        {
            var actual = new Scheduler();

            Assert.Throws<ArgumentNullException>(() => actual.AddTask(Guid.NewGuid(), null, 5, false));
        }

        [Fact]
        public void AddTaskToTheQueueAndStartShouldCallTheExecuteFunctionAtLeastOnce()
        {
            var actual = new TaskMock();
            var taskId = Guid.NewGuid();
            const int pollingInterval = 5;
            const bool isActive = true;

            var scheduler = new Scheduler();
            var exceptionList = new ConcurrentBag<Exception>();
            scheduler.TaskException += (sender, exception) => exceptionList.Add(exception);

            scheduler.AddTask(taskId, actual, pollingInterval, isActive);
            scheduler.Start();
            Thread.Sleep(100);
            scheduler.Stop();
            Thread.Sleep(200);

            Assert.Equal(0, exceptionList.Count);
            actual.AssertExecuteCallsAtLeast(1);
        }

        [Fact]
        public void AddExceptionThrowingTaskShouldIndicateAnExceptionInTheProperties()
        {
            var actual = new TaskMock().SetExecuteException<Exception>();
            var taskId = Guid.NewGuid();
            const int pollingInterval = 5;
            const bool isActive = true;

            var scheduler = new Scheduler();
            var exceptionList = new ConcurrentBag<Exception>();
            scheduler.TaskException += (sender, exception) => exceptionList.Add(exception);

            scheduler.AddTask(taskId, actual, pollingInterval, isActive);
            scheduler.Start();
            Thread.Sleep(1000);
            scheduler.Stop();

            actual.AssertExecuteCallsAtLeast(1);
            Assert.Equal(1, exceptionList.Count);
            Assert.Equal(typeof(Exception), exceptionList.Single().InnerException.GetType());
        }

        [Fact]
        public void StartingStoppingAndThenStartingAgainAllowsTaskToRun()
        {
            var actual = new TaskMock();
            var taskId = Guid.NewGuid();
            const int pollingInterval = 5;
            const bool isActive = true;

            var scheduler = new Scheduler();
            var exceptionList = new ConcurrentBag<Exception>();
            scheduler.TaskException += (sender, exception) => exceptionList.Add(exception);

            scheduler.AddTask(taskId, actual, pollingInterval, isActive);
            scheduler.Start();
            Thread.Sleep(100);
            var initialInvocationCount = actual.ExecuteInvocationCount;

            Assert.True(initialInvocationCount > 0);

            scheduler.Stop();
            Thread.Sleep(100);
            scheduler.Start();
            Thread.Sleep(100);
            scheduler.Stop();
            Thread.Sleep(100);
            var finalInvocationCount = actual.ExecuteInvocationCount;

            Assert.True(finalInvocationCount > initialInvocationCount);

            Assert.Equal(0, exceptionList.Count);
        }

        [Fact]
        public void StartingWithAnInactiveTaskAndThenActivatingThatTaskForcesTaskToRun()
        {
            var actual = new TaskMock();
            var taskId = Guid.NewGuid();
            const int pollingInterval = 5;
            const bool isActive = false;

            var scheduler = new Scheduler();
            var exceptionList = new ConcurrentBag<Exception>();
            scheduler.TaskException += (sender, exception) => exceptionList.Add(exception);

            scheduler.AddTask(taskId, actual, pollingInterval, isActive);
            scheduler.Start();
            Thread.Sleep(100);

            Assert.Equal(0, actual.ExecuteInvocationCount);

            scheduler.ResumeTask(taskId);
            Thread.Sleep(100);
            scheduler.Stop();
            Thread.Sleep(100);

            Assert.Equal(0, exceptionList.Count);
            actual.AssertExecuteCallsAtLeast(1);
        }

        [Fact]
        public void StartingWithAnActiveTaskAndThenSuspendingThatTaskForcesTheTaskToStopRunning()
        {
            var actual = new TaskMock();
            var taskId = Guid.NewGuid();
            const int pollingInterval = 5;
            const bool isActive = true;

            var scheduler = new Scheduler();
            var exceptionList = new ConcurrentBag<Exception>();
            scheduler.TaskException += (sender, exception) => exceptionList.Add(exception);

            scheduler.AddTask(taskId, actual, pollingInterval, isActive);
            scheduler.Start();
            Thread.Sleep(100);
            scheduler.SuspendTask(taskId);
            Thread.Sleep(100);

            actual.AssertExecuteCallsAtLeast(1);

            var initialInvocationCount = actual.ExecuteInvocationCount;
            Thread.Sleep(100);
            var finalInvocationCount = actual.ExecuteInvocationCount;

            Assert.Equal(finalInvocationCount, initialInvocationCount);

            Assert.Equal(0, exceptionList.Count);
        }

        [Fact]
        public void ChangingPollingIntervalOnActiveTaskChangesTheIntervalAndKeepsRunning()
        {
            ChangePollingIntervalOnRunningScheduler(isTaskActive: true, isSchedulerRunning: true);
        }

        [Fact]
        public void ChangingPollingIntervalOnAnInactiveTaskChangesTheIntervalButDoesntRun()
        {
            ChangePollingIntervalOnRunningScheduler(isTaskActive: false, isSchedulerRunning: true);
        }

        [Fact]
        public void ChangingPollingIntervalForInactiveTaskWhenSchedulerIsNotRunningChangesIntervalButDoesntRunTask()
        {
            ChangePollingIntervalOnRunningScheduler(isTaskActive: false, isSchedulerRunning: false);
        }

        [Fact]
        public void ChangingPollingIntervalForActiveTaskWhenSchedulerIsNotRunningChangesIntervalButDoesntRunTask()
        {
            ChangePollingIntervalOnRunningScheduler(isTaskActive: true, isSchedulerRunning: false);
        }

        private void ChangePollingIntervalOnRunningScheduler(
            bool isTaskActive,
            bool isSchedulerRunning)
        {
            var actual = new TaskMock();
            var taskId = Guid.NewGuid();
            const int originalPollingInterval = 5;
            const int finalPollingInterval = 10;
            var expectedRepetitionPeriod = TimeSpan.FromMilliseconds(finalPollingInterval);

            var scheduler = new Scheduler();
            var exceptionList = new ConcurrentBag<Exception>();
            scheduler.TaskException += (sender, exception) => exceptionList.Add(exception);

            scheduler.AddTask(
                taskId, 
                actual, 
                originalPollingInterval, 
                isTaskActive);

            if (isSchedulerRunning)
            {
                scheduler.Start();
            }

            scheduler.UpdatePollingInterval(
                taskId, 
                finalPollingInterval);

            var actualIsSchedulerRunning = scheduler.GetMemberValue<bool>("_isStarted");

            Assert.Equal(0, exceptionList.Count());
            Assert.Equal(isSchedulerRunning, actualIsSchedulerRunning);

            var task = scheduler
                .GetMemberValue<List<RepeatingTask>>("_tasks")
                .SingleOrDefault(t => t.TaskId == taskId);
            Assert.NotNull(task);

            var actualIsTaskActive = task.GetMemberValue<bool>("_isActive");
            var actualRepetitionPeriod = task.GetMemberValue<TimeSpan>("_repetitionPeriod");
            Assert.Equal(isTaskActive, actualIsTaskActive);
            Assert.Equal(
                expectedRepetitionPeriod,
                actualRepetitionPeriod);

            if (isSchedulerRunning && isTaskActive)
            {
                Assert.NotNull(task.GetMemberValue<CancellationTokenSource>("_tokenSource"));
            }

            if (isSchedulerRunning && isTaskActive)
            {
                actual.AssertExecuteCallsAtLeast(1);
            }
            else
            {
                actual.AssertNoExecuteCalls();
            }
        }

        [Fact]
        public void ForcingCausesAllActiveTasksToRunExactlyOnce()
        {
            var taskId1 = Guid.NewGuid();
            var taskInterval1 = 5;
            var isTask1Active = true;
            var taskMock1 = new TaskMock();

            var taskId2 = Guid.NewGuid();
            var taskInterval2 = 10;
            var isTask2Active = true;
            var taskMock2 = new TaskMock();

            var taskId3 = Guid.NewGuid();
            var taskInterval3 = 15;
            var isTask3Active = true;
            var taskMock3 = new TaskMock();

            var scheduler = new Scheduler();
            scheduler.AddTask(taskId1, taskMock1, taskInterval1, isTask1Active);
            scheduler.AddTask(taskId2, taskMock2, taskInterval2, isTask2Active);
            scheduler.AddTask(taskId3, taskMock3, taskInterval3, isTask3Active);

            scheduler.Force();

            Assert.Equal(isTask1Active ? 1 : 0, taskMock1.ExecuteInvocationCount);
            Assert.Equal(isTask2Active ? 1 : 0, taskMock2.ExecuteInvocationCount);
            Assert.Equal(isTask3Active ? 1 : 0, taskMock3.ExecuteInvocationCount);

        }

        [Fact]
        public void ForcingCausesAllExceptionThrowingTasksToRecordExceptions()
        {
            var exceptions = new ConcurrentBag<Exception>();

            var taskId1 = Guid.NewGuid();
            var taskInterval1 = 5;
            var isTask1Active = true;
            var taskMock1 = new TaskMock();

            var taskId2 = Guid.NewGuid();
            var taskInterval2 = 10;
            var isTask2Active = true;
            var taskMock2 = new TaskMock();
            taskMock2.SetExecuteException<Exception>();

            var scheduler = new Scheduler();
            scheduler.AddTask(taskId1, taskMock1, taskInterval1, isTask1Active);
            scheduler.AddTask(taskId2, taskMock2, taskInterval2, isTask2Active);

            scheduler.TaskException += (sender, exception) => exceptions.Add(exception);

            scheduler.Force();

            Assert.Equal(isTask1Active ? 1 : 0, taskMock1.ExecuteInvocationCount);
            Assert.Equal(1, exceptions.Count);
        }
    }
}
