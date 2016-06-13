using System;
using Aggregator.Framework.Contracts;
using Aggregator.Framework.Logging;

namespace Aggregator.Configuration
{
    public class ConsoleLogger : ILogger
    {
        private readonly LoggingSettings _settings;

        public ConsoleLogger(LoggingSettings settings)
        {
            _settings = settings;
        }

        public void Error(string message)
        {
            if (_settings.LogErrors)
            {
                Console.WriteLine($"[ERROR] [{DateTime.Now}] {message}");
            }
        }

        public void Info(string message)
        {
            if (_settings.LogInfos)
            {
                Console.WriteLine($"[INFO] [{DateTime.Now}] {message}");
            }
        }
    }
}
