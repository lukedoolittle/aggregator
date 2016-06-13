using System;
using SimpleCQRS.Domain;

namespace Aggregator.Framework.Logging
{
    public class LogEntry : Entity
    {
        public LogEntry(
            LogTypeEnum logType, 
            DateTime timestamp, 
            string message)
        {
            LogType = logType;
            Timestamp = timestamp;
            Message = message;

            Id = Guid.NewGuid();
        }

        public LogTypeEnum LogType { get; }
        public DateTime Timestamp { get; }
        public string Message { get; }
        public Guid Id { get; }
    }

    public enum LogTypeEnum
    {
        Info,
        Error
    }
}
