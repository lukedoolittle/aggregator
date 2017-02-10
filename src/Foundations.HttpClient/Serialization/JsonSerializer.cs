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
        //This serializer could be customized and it would solve a lot of the 
        //problems with the serializable hacks in some of the classes
        public string Serialize(object entity)
        {
            return Serialize(entity, null);
        }

        public string Serialize(
            object entity, 
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

            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(
                    entity.GetType(),
                    settings);
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

