namespace Aggregator.Framework.Contracts
{
    public interface ITask
    {
        System.Threading.Tasks.Task Execute(object parameter = null);
    }
}
