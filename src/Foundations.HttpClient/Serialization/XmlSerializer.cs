using System;

namespace Foundations.HttpClient.Serialization
{
    public class XmlSerializer : ISerializer
    {
        public string Serialize<T>(T item)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(
            string item, 
            string datetimeFormat = null)
        {
            throw new NotImplementedException();
        }
    }
}
