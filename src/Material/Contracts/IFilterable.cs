namespace Material.Contracts
{
    public interface IFilterable : ITimeSeries
    {
        string RequestFilterKey { get; }
        string ResponseFilterKey { get; }
    }
}
