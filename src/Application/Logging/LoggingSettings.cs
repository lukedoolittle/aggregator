namespace Aggregator.Framework.Logging
{
    public class LoggingSettings
    {
        public LoggingSettings(
            bool logErrors, 
            bool logInfos)
        {
            LogErrors = logErrors;
            LogInfos = logInfos;
        }

        public bool LogErrors { get; }
        public bool LogInfos { get; }
    }
}
