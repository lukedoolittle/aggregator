namespace Material.Framework.Serialization
{
    public interface ISerializer
    {
        string Serialize(object entity);
        string Serialize(object entity, string dateTimeFormat);
        TEntity Deserialize<TEntity>(string entity);
        TEntity Deserialize<TEntity>(string entity, string dateTimeFormat);
    }
}
