using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Foundations.HttpClient.Serialization
{
    public class JsonSerializer : ISerializer
    {
        //TODO: customize this serializer
        //http://stackoverflow.com/questions/794838/datacontractjsonserializer-and-enums
        public string Serialize(object entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(
                    entity.GetType());
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

        public string Serialize<TEntity>(TEntity entity)
        {
            return Serialize((object)entity);
        }

        public TEntity Deserialize<TEntity>(
            string entity, 
            string dateTimeFormat)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var settings = new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true
            };
            if (dateTimeFormat != null)
            {
                settings.DateTimeFormat = new DateTimeFormat(
                    dateTimeFormat, 
                    CultureInfo.InvariantCulture);
            }
            var serializer = new DataContractJsonSerializer(
                typeof(TEntity),
                settings);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(entity)))
            {
                return (TEntity)serializer.ReadObject(stream);
            }
        }

        public TEntity Deserialize<TEntity>(string entity)
        {
            return Deserialize<TEntity>(entity, null);
        }
    }
}

