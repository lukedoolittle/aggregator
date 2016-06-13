using System;
using Aggregator.Framework.Contracts;
using SimpleCQRS.Framework.Contracts;

namespace Aggregator.Framework.Logging
{
    public class DatabaseLogger : ILogger
    {
        private readonly IDatabase<LogEntry> _database;
        private readonly LoggingSettings _settings;

        public DatabaseLogger(
            IDatabase<LogEntry> database, 
            LoggingSettings settings)
        {
            _database = database;
            _settings = settings;
        }

        public void Error(string message)
        {
            if (_settings.LogErrors)
            {
                StoreLogEntry(
                    message,
                    LogTypeEnum.Error);
            }
        }

        public void Info(string message)
        {
            if (_settings.LogInfos)
            {
                StoreLogEntry(
                    message,
                    LogTypeEnum.Info);
            }
        }

        private void StoreLogEntry(
            string message, 
            LogTypeEnum logType)
        {
            _database.Update(
                new LogEntry(
                    logType, 
                    DateTime.Now, 
                    message));
        }
    }
}
