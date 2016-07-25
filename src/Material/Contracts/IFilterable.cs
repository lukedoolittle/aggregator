namespace Material.Contracts
{
    public interface IFilterable : ITimeseries
    {
        string RequestFilterKey { get; }
        string ResponseFilterKey { get; }
    }
}
