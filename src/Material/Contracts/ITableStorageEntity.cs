namespace Material.Contracts
{
    public interface ITableStorageEntity
    {
        string PartitionKey { get; }

        string RowKey { get;}
    }
}
