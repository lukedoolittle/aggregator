﻿namespace Foundations.HttpClient.Serialization
{
    public interface ISerializer
    {
        string Serialize(object entity);
        TEntity Deserialize<TEntity>(string entity, string datetimeFormat = null);
    }
}
