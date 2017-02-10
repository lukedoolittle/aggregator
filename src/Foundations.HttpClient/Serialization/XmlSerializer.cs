using System;

namespace Foundations.HttpClient.Serialization
{
    public class XmlSerializer : ISerializer
    {
        public string Serialize(object entity)
        {
            throw new NotImplementedException();
        }

        public string Serialize(
            object entity, 
            string dateTimeFormat)
        {
            throw new NotImplementedException();
        }

        public TEntity Deserialize<TEntity>(
            string entity, 
            string dateTimeFormat)
        {
            throw new NotImplementedException();
        }

        public TEntity Deserialize<TEntity>(string entity)
        {
            return Deserialize<TEntity>(entity, null);
        }
    }
}
