namespace Foundations.HttpClient.Serialization
{
    public interface ISerializer
    {
        string Serialize<TEntity>(TEntity entity);
        TEntity Deserialize<TEntity>(string entity);
    }
}
