namespace Material.Contracts
{
    public interface ITableStorageEntity
    {
        string PartitionKey { get; set; }

        string RowKey { get; set; }
    }
}
