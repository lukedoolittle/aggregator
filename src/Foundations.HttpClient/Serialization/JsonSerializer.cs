using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Foundations.HttpClient.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize<TEntity>(TEntity entity)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(
                    typeof(TEntity));
                serializer.WriteObject(
                    memoryStream, 
                    entity);

                var streamArray = memoryStream.ToArray();
                return Encoding.UTF8.GetString(
                    streamArray, 
                    0, 
                    streamArray.Length);
            }
        }

        public TEntity Deserialize<TEntity>(string entity)
        {
            var serializer = new DataContractJsonSerializer(typeof(TEntity));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(entity)))
            {
                return (TEntity)serializer.ReadObject(stream);
            }
        }
    }
}

