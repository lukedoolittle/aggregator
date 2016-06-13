namespace Aggregator.Framework.Contracts
{
    public interface ILogger
    {
        void Error(string message);
        void Info(string message);
    }
}
